using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Infrastructure;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.Config;
using Game.Scripts.Tutorial;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Gameplay.Pickaxe
{
  public class PickaxesFloorView : MonoBehaviour
  {
    [SerializeField] private List<PickaxeRootView> pickaxeRoots;
    [SerializeField] private List<ChestConfig> chests;
    [SerializeField] private bool canSpawnChest;
    private ConfigProvider _configProvider;
    private TutorialService _tutorialService;
    private float _chestChance;

    private void Awake()
    {
      _configProvider = ServiceProvider.Get<ConfigProvider>();
      _tutorialService = ServiceProvider.Get<TutorialService>();
    }

    public void SetChestChance(float chestChance)
    {
      _chestChance = chestChance;
    }

    public bool IsFull()
    {
      foreach (var pickaxeRoot in pickaxeRoots)
      {
        if (pickaxeRoot.IsEmpty())
          return false;
      }
      
      return true;
    }

    public void AddView(PickaxeView view)
    {
      gameObject.SetActive(true);
      
      var availableRoot = pickaxeRoots.FirstOrDefault(r => r.IsEmpty());
      availableRoot?.AddView(view);
    }

    public List<PickaxeView> ClearViews()
    {
      var result = new List<PickaxeView>();
      foreach (var root in pickaxeRoots)
      {
        if (root.IsEmpty())
          continue;
        
        result.Add(root.RemoveView());
      }
      
      gameObject.SetActive(false);
      
      return result;
    }

    public void Punch()
    {
      foreach (var root in pickaxeRoots)
      {
        if (root.IsEmpty())
          continue;

        if (TryGetChest(out var chest))
          root.PunchChest(chest).Forget();
        else 
          root.PunchOre().Forget();
      }
    }

    private bool TryGetChest(out ChestConfig chest)
    {
      if (!CanSpawnChest())
      {
        chest = null;
        return false;
      }

      chest = GetRandomChest();
      return true;
    }

    private bool CanSpawnChest()
    {
      var result = true;
      result &= canSpawnChest;
      result &= chests.Count > 0;
      result &= Random.value < _chestChance;
      result &= _tutorialService.IsCompleted(TutorialType.StartingTutorial);

      return result;
    }

    private ChestConfig GetRandomChest()
    {
      var weights = new List<WeightedItem<ChestConfig>>();
      foreach (var chestConfig in chests)
      {
        weights.Add(new WeightedItem<ChestConfig>(chestConfig, _configProvider.ChestChances[chestConfig.Type]));
      }

      return weights.GetWeightedRandom();
    }
  }
}