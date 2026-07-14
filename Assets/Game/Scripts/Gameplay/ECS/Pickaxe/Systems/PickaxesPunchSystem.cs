using Game.Scripts.Gameplay.ECS.Ore.Aspects;
using Game.Scripts.Gameplay.ECS.Pickaxe.Aspects;
using Game.Scripts.Gameplay.ECS.Pickaxe.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickaxe.Systems
{
  public class PickaxesPunchSystem : IProtoInitSystem, IProtoRunSystem
  {
    private PickaxeAspect _pickaxeAspect;
    private OreAspect _oreAspect;
    private ProtoIt _eventEntities;
    private ProtoIt _pickaxeMineEntities;
    
    public void Init(IProtoSystems systems)
    {
      _pickaxeAspect = systems.GetAspect<PickaxeAspect>();
      _oreAspect = systems.GetAspect<OreAspect>();
      _eventEntities = Entities.ProtoIt<PickaxesPunchEvent>(systems.World());
      _pickaxeMineEntities = Entities.ProtoIt<PickaxeMineComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var eventEntity in _eventEntities)
      {
        if (_oreAspect.OrePool.Len() > 150)
          continue;
       
        var ownerId = _pickaxeAspect.PickaxesPunchEvents.Get(eventEntity).OwnerId;
        foreach (var pickaxeMineEntity in _pickaxeMineEntities)
        {
          var mine = _pickaxeAspect.PickaxeMines.Get(pickaxeMineEntity);
          if (mine.OwnerId != ownerId)
            continue;

          mine.PickaxesHolder.PickaxesPunch();
        }
      }
    }
  }
}