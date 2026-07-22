using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.Common.Components;
using Game.Scripts.Gameplay.ECS.Pickup.Components;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Pickup.Systems
{
  public class PickupSystem : IProtoInitSystem, IProtoRunSystem
  {
    private PickupAspect _pickupAspect;
    private TransformAspect _transformAspect;
    private ProtoIt _collectorEntities;
    private ProtoIt _itemEntities;

    public void Init(IProtoSystems systems)
    {
      _pickupAspect = systems.GetAspect<PickupAspect>();
      _transformAspect = systems.GetAspect<TransformAspect>();
      _collectorEntities = Entities.ProtoIt<PickupCollectorComponent, TransformComponent>(systems.World());
      _itemEntities = Entities.ProtoIt<PickupItemComponent, TransformComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var collectorEntity in _collectorEntities)
      {
        var collectorTransform = _transformAspect.TransformsPool.Get(collectorEntity).Transform;
        ref var collector = ref _pickupAspect.PickupCollectorsPool.Get(collectorEntity);
        
        foreach (var itemEntity in _itemEntities)
        {
          var oreTransform = _transformAspect.TransformsPool.Get(itemEntity).Transform;
          if (!CanPickup(collectorTransform.position, oreTransform.position, collector.PickupDistance))
            continue;
          
          ref var component = ref _pickupAspect.PickupEventsPool.Add(itemEntity);
          component.PickupCollector = collector.PickupCollector;
        }
      }
    }

    private bool CanPickup(Vector3 itemPosition, Vector3 collectorPosition, float pickupDistance)
    {
      return Vector3.Distance(itemPosition, collectorPosition) < pickupDistance;
    }
  }
}