using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay.ECS;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Gameplay.Pickaxe
{
  public class PickaxeView : MonoBehaviour
  {
    private static readonly int Punch = Animator.StringToHash("Punch");
    
    [SerializeField] private Transform oreSpawnPoint;
    [SerializeField] private Animator animator;
    private PickaxeConfig _pickaxeConfig;

    public void Init(PickaxeConfig pickaxeConfig)
    {
      _pickaxeConfig = pickaxeConfig;
    }
    
    public PickaxeType GetPickaxeType()
    {
      return _pickaxeConfig.pickaxeType;
    }
    
    public async UniTask PlayPunchAnimation(CancellationToken token)
    {
      var delay = Random.Range(0, 0.2f);
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