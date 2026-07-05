using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.Common.Components;
using Game.Scripts.Gameplay.ECS.Destroy;
using Game.Scripts.Gameplay.ECS.Pickup.Components;
using Game.Scripts.Infrastructure.Services;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Pickup.Systems
{
  public class PickupOreSystem : IProtoInitSystem, IProtoRunSystem
  {
    private PickupAspect _pickupAspect;
    private DestroyAspect _destroyAspect;
    private TransformAspect _transformAspect;
    private ProtoIt _oreEntities;
    private ProtoIt _collectorEntities;
    private EconomyService _economyService;

    public void Init(IProtoSystems systems)
    {
      _pickupAspect = systems.GetAspect<PickupAspect>();
      _destroyAspect = systems.GetAspect<DestroyAspect>();
      _transformAspect = systems.GetAspect<TransformAspect>();
      _oreEntities = Entities.ProtoIt<PickupOreItemComponent, TransformComponent>(systems.World());
      _collectorEntities = Entities.ProtoIt<PickupOreCollectorComponent, TransformComponent>(systems.World());
      _economyService = ServiceProvider.Get<EconomyService>();
    }

    public void Run()
    {
      foreach (var collectorEntity in _collectorEntities)
      {
        var collectorTransform = _transformAspect.TransformsPool.Get(collectorEntity).Transform;
        var pickupDistance = _pickupAspect.PickupOreCollectorsPool.Get(collectorEntity).PickupDistance;
        
        foreach (var oreEntity in _oreEntities)
        {
          var oreTransform = _transformAspect.TransformsPool.Get(oreEntity).Transform;
          if (!CanPickup(collectorTransform.position, oreTransform.position, pickupDistance))
            continue;
          
          var amount = _pickupAspect.PickupOreItemsPool.Get(oreEntity).Amount;
          _economyService.AddOre(amount);
          
          _destroyAspect.DestroyPool.Add(oreEntity);
        }
      }
    }

    private bool CanPickup(Vector3 itemPosition, Vector3 collectorPosition, float pickupDistance)
    {
      return Vector3.Distance(itemPosition, collectorPosition) < pickupDistance;
    }
  }
}