using Game.Scripts.Gameplay.ECS.Pickaxe.Aspects;
using Game.Scripts.Gameplay.ECS.Pickaxe.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickaxe.Systems
{
  public class RebuildPickaxesSystem : IProtoInitSystem, IProtoRunSystem
  {
    private PickaxeAspect _pickaxeAspect;
    private ProtoIt _eventEntities;
    private ProtoIt _minesEntities;
    
    public void Init(IProtoSystems systems)
    {
      _pickaxeAspect = systems.GetAspect<PickaxeAspect>();
      _eventEntities = Entities.ProtoIt<RebuildPickaxeEvent>(systems.World());
      _minesEntities = Entities.ProtoIt<PickaxeMineComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _eventEntities)
      {
        var spawnEvent = _pickaxeAspect.RebuildPickaxeEvents.Get(entity);
        foreach (var mineEntity in _minesEntities)
        {
          var mine = _pickaxeAspect.PickaxeMines.Get(mineEntity);
          if (spawnEvent.OwnerID != mine.OwnerId)
            continue;

          mine.PickaxesHolder.RebuildPickaxes(spawnEvent.Pickaxes);
        }
      }
    }
  }
}