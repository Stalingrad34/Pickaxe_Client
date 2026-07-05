using Game.Scripts.Gameplay.ECS.EntityConvert.Converters;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Pickup.Converters
{
  public class PickupOreItemConverter : MonoBehaviour, IEntityConverter
  {
    [SerializeField] private int amount;
    
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<PickupAspect>();
      ref var component = ref aspect.PickupOreItemsPool.Add(entity);
      component.Amount = (ulong)amount;
    }
  }
}