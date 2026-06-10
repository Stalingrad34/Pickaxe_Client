using Game.Scripts.Gameplay.ECS.Pickaxe.Aspects;
using Game.Scripts.Gameplay.ECS.Pickaxe.Components;
using Game.Scripts.Gameplay.ECS.Spawn.Aspects;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Spawn.Systems
{
  public class LinkMineSystem : IProtoInitSystem, IProtoRunSystem
  {
    private SpawnAspect _spawn;
    private PickaxeAspect _pickaxe;
    private ProtoIt _eventEntities;
    private ProtoIt _mineEntities;
    
    public void Init(IProtoSystems systems)
    {
      _spawn = systems.GetAspect<SpawnAspect>();
      _pickaxe = systems.GetAspect<PickaxeAspect>();
      _eventEntities = Entities.ProtoIt<SpawnCharacterEvent>(systems.World());
      _mineEntities = Entities.ProtoIt<PickaxeMineComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var eventEntity in _eventEntities)
      {
        ref var spawnEvent = ref _spawn.SpawnCharacterEvents.Get(eventEntity);
        foreach (var mineEntity in _mineEntities)
        {
          ref var mine = ref _pickaxe.PickaxeMines.Get(mineEntity);
          if (!string.IsNullOrEmpty(mine.OwnerId))
            continue;

          mine.OwnerId = spawnEvent.Data.Id;
          spawnEvent.Data.Position = mine.PlayerSpawnPosition.position;
          spawnEvent.Data.StartAngleY = mine.PlayerSpawnPosition.eulerAngles.y;
        }
      }
    }
  }
}