using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay.ECS;
using Game.Scripts.Infrastructure.Custom;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Gameplay.Pickaxe
{
  public class PickaxeView : MonoBehaviour
  {
    private static readonly int Punch = Animator.StringToHash("Punch");
    
    [SerializeField] private Transform oreSpawnPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private CustomText pickaxeName;
    [SerializeField] private CustomText pickaxeLevel;
    private PickaxeConfig _pickaxeConfig;

    public void Init(PickaxeConfig pickaxeConfig)
    {
      _pickaxeConfig = pickaxeConfig;
      pickaxeName.SetText(_pickaxeConfig.nameKey);
      pickaxeName.color = pickaxeConfig.oreConfig.pickupColor;
      
      var level = (int) pickaxeConfig.pickaxeType;
      pickaxeLevel.SetText(new TextData("pickaxe_level", level.ToString()));
    }
    
    public PickaxeType GetPickaxeType()
    {
      return _pickaxeConfig.pickaxeType;
    }
    
    public async UniTask PlayPunchAnimation(CancellationToken token)
    {
      var delay = Random.Range(0, 0.5f);
      await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token).SuppressCancellationThrow();
      if (token.IsCancellationRequested)
        return;
      
      animator.SetTrigger(Punch);
    }

    public void SpawnOre()
    {
      ECSRunner.EcsEventWriter.SpawnOre(oreSpawnPoint.position, transform.position - oreSpawnPoint.position, _pickaxeConfig.oreConfig);
    }
  }
}