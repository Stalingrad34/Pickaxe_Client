using Game.Scripts.Gameplay.ECS.EntityConvert.Converters;
using Game.Scripts.Gameplay.ECS.Pickup.Interfaces;
using Game.Scripts.Gameplay.Units;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Pickup.Converters
{
  public class PickupCollectorConverter : MonoBehaviour, IEntityConverter, IUnitSetup
  {
    [SerializeField] private float pickupDistance;
    private IPickupCollector _collector;

    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<PickupAspect>();
      ref var component = ref aspect.PickupCollectorsPool.Add(entity);
      component.PickupDistance = pickupDistance;
      component.PickupCollector = _collector;
    }

    public void Setup(UnitData data)
    {
      _collector = data.Collector;
    }
  }
}