using System;
using Cysharp.Threading.Tasks;
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
    
    public async UniTaskVoid PlayPunchAnimation()
    {
      var delay = Random.Range(0, 0.5f);
      await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: destroyCancellationToken).SuppressCancellationThrow();
      if (destroyCancellationToken.IsCancellationRequested)
        return;
      
      animator.SetTrigger(Punch);
    }

    public void SpawnOre()
    {
      var oreView = Instantiate(_pickaxeConfig.oreConfig.prefab, oreSpawnPoint.position, Quaternion.identity);
      oreView.Init(_pickaxeConfig.oreConfig);
    }
  }
}