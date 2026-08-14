using System.Collections.Generic;
using System.Text;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Infrastructure;
using UnityEngine;

namespace Game.Scripts.Gameplay.Chest
{
  [CreateAssetMenu(menuName = "Data/ChestTest")]
  public class ChestTest : ScriptableObject
  {
    [SerializeField] private List<ChestConfig> configs;
    [SerializeField] private float chestChance;
    [SerializeField] private int seconds;
    
    private SortedDictionary<PickaxeType, int> _pickaxes = new ();
    
    [ContextMenu("Test")]
    public void Test()
    {
      _pickaxes.Clear();
      for (int i = 0; i < seconds; i++)
      {
        if (Random.value > chestChance)
          continue;
       
        var chest = GetRandomChest(configs);
        var pickaxe = GetRandomPickaxeConfig(chest);

        _pickaxes.TryAdd(pickaxe.pickaxeType, 0);
        _pickaxes[pickaxe.pickaxeType]++;
      }

      var log = new StringBuilder();
      foreach (var pair in _pickaxes)
      {
        log.AppendLine($"{pair.Key}: {pair.Value}");
      }
      
      Debug.Log(log.ToString());
    }
    
    private PickaxeConfig GetRandomPickaxeConfig(ChestConfig chestConfig)
    {
      var items = new List<WeightedItem<PickaxeConfig>>();
      foreach (var variant in chestConfig.Variants)
      {
        items.Add(new WeightedItem<PickaxeConfig>(variant.pickaxeConfig, variant.Chance));
      }

      return items.GetWeightedRandom();
    }
    
    private ChestConfig GetRandomChest(List<ChestConfig> chests)
    {
      var weights = new List<WeightedItem<ChestConfig>>();
      foreach (var chestConfig in chests)
      {
        weights.Add(new WeightedItem<ChestConfig>(chestConfig, chestConfig.Weight));
      }

      return weights.GetWeightedRandom();
    }
  }
}