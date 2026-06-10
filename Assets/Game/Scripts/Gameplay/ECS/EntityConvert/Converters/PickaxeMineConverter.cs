using Game.Scripts.Gameplay.ECS.Pickaxe.Aspects;
using Game.Scripts.Gameplay.Pickaxe;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Converters
{
  public class PickaxeMineConverter : MonoBehaviour, IEntityConverter
  {
    [SerializeField] private Transform playerSpawnPosition;
    [SerializeField] private PickaxesHolder pickaxesHolder;
    
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<PickaxeAspect>();
      ref var component = ref aspect.PickaxeMines.Add(entity);
      component.PickaxesHolder = pickaxesHolder;
      component.PlayerSpawnPosition =  playerSpawnPosition;
    }
  }
}