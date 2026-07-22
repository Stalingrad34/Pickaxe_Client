using Game.Scripts.Gameplay.ECS.Destroy.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Destroy.Systems
{
  public class DestroyEntitySystem : IProtoInitSystem, IProtoRunSystem
  {
    private DestroyAspect _destroyAspect;
    private ProtoIt _entities;

    public void Init(IProtoSystems systems)
    {
      _destroyAspect = systems.GetAspect<DestroyAspect>();
      _entities = Entities.ProtoIt<DestroyEntityEvent>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _entities)
      {
        _destroyAspect.World().DelEntity(entity);
      }
    }
  }
}