using Game.Scripts.Gameplay.ECS.Pickaxe.Aspects;
using Game.Scripts.Gameplay.ECS.Pickaxe.Components;
using Game.Scripts.Infrastructure;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickaxe.Systems
{
  public class SpawnPickaxeSystem : IProtoInitSystem, IProtoRunSystem
  {
    private PickaxeAspect _pickaxeAspect;
    private ProtoIt _entities;
    
    public void Init(IProtoSystems systems)
    {
      _pickaxeAspect = systems.GetAspect<PickaxeAspect>();
      _entities = Entities.ProtoIt<SpawnPickaxeEvent>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _entities)
      {
        var spawnEvent = _pickaxeAspect.SpawnPickaxeEvents.Get(entity);
        var pickaxeData = AssetProvider.GetPickaxeData(spawnEvent.PickaxeDataPath);
      }
    }
  }
}