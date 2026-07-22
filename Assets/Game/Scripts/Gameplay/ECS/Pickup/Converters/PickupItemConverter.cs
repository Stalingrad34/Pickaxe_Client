using Game.Scripts.Gameplay.ECS.EntityConvert.Converters;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Pickup.Converters
{
  public class PickupItemConverter : MonoBehaviour, IEntityConverter
  {
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<PickupAspect>();
      aspect.PickupItemsPool.Add(entity);
    }
  }
}