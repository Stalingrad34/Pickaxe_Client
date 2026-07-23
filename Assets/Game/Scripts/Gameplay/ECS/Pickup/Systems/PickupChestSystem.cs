using Game.Scripts.Gameplay.ECS.Common.Components;
using Game.Scripts.Gameplay.ECS.Destroy;
using Game.Scripts.Gameplay.ECS.Pickup.Components;
using Game.Scripts.Gameplay.ECS.Spawn;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickup.Systems
{
  public class PickupChestSystem : IProtoInitSystem, IProtoRunSystem
  {
    private PickupAspect _pickupAspect;
    private SpawnAspect _spawnAspect;
    private DestroyAspect _destroyAspect;
    private ProtoIt _chestEntities;
    private ProtoIt _gameSessionEntities;

    public void Init(IProtoSystems systems)
    {
      _pickupAspect = systems.GetAspect<PickupAspect>();
      _spawnAspect = systems.GetAspect<SpawnAspect>();
      _destroyAspect = systems.GetAspect<DestroyAspect>();
      _chestEntities = Entities.ProtoIt<ChestComponent, PickupEvent>(systems.World());
      _gameSessionEntities = Entities.ProtoIt<GameSessionComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var chestEntity in _chestEntities)
      {
        var chest = _spawnAspect.ChestPool.Get(chestEntity);
        var collector = _pickupAspect.PickupEventsPool.Get(chestEntity).PickupCollector;

        if (!collector.CanTake())
          continue;
       
        collector.Take(chest.ChestView);
        
        _destroyAspect.DestroyEntitiesPool.Add(chestEntity);
        
        var (entity, success) = _gameSessionEntities.FirstSlow();
        if (!success)
          continue;
        
        ref var gameSession = ref _spawnAspect.GameSessionPool.Get(entity);
        gameSession.MainGUIModel.ShowChestInfo(collector, chest.ChestConfig);
      }
    }
  }
}