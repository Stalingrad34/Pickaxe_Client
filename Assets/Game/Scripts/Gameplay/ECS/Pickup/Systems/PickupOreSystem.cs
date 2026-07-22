using Game.Scripts.Gameplay.ECS.Destroy;
using Game.Scripts.Gameplay.ECS.Pickup.Components;
using Game.Scripts.Gameplay.ECS.Spawn;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Game.Scripts.Infrastructure.Services;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickup.Systems
{
  public class PickupOreSystem : IProtoInitSystem, IProtoRunSystem
  {
    private PickupAspect _pickupAspect;
    private SpawnAspect _spawnAspect;
    private DestroyAspect _destroyAspect;
    private ProtoIt _oreEntities;
    private EconomyService _economyService;

    public void Init(IProtoSystems systems)
    {
      _pickupAspect = systems.GetAspect<PickupAspect>();
      _spawnAspect = systems.GetAspect<SpawnAspect>();
      _destroyAspect = systems.GetAspect<DestroyAspect>();
      _oreEntities = Entities.ProtoIt<OreComponent, OreRewardComponent, PickupEvent>(systems.World());
      _economyService = ServiceProvider.Get<EconomyService>();
    }

    public void Run()
    {
      foreach (var oreEntity in _oreEntities)
      {
        var amount = _pickupAspect.PickupOreRewardsPool.Get(oreEntity).Amount;
        var color = _pickupAspect.PickupOreRewardsPool.Get(oreEntity).PickupTextColor;
        var view = _spawnAspect.OrePool.Get(oreEntity).OreView;
          
        _economyService.AddOre(amount, color);
        ref var component = ref _destroyAspect.DestroyGameObjectsPool.Add(oreEntity);
        component.GameObject = view.gameObject;
      }
    }
  }
}