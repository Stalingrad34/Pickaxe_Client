using Game.Scripts.Gameplay.ECS.EntityConvert.Converters;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Pickup.Converters
{
  public class PickupOreCollectorConverter : MonoBehaviour, IEntityConverter
  {
    [SerializeField] private float pickupDistance;
    
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<PickupAspect>();
      ref var component = ref aspect.PickupOreCollectorsPool.Add(entity);
      component.PickupDistance = pickupDistance;
    }
  }
}