using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.Common.Components;
using Game.Scripts.Gameplay.ECS.Destroy.Components;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Destroy.Systems
{
  public class DestroyGameObjectSystem : IProtoInitSystem, IProtoRunSystem
  {
    private DestroyAspect _destroyAspect;
    private TransformAspect _transformAspect;
    private ProtoIt _entities;

    public void Init(IProtoSystems systems)
    {
      _destroyAspect = systems.GetAspect<DestroyAspect>();
      _transformAspect = systems.GetAspect<TransformAspect>();
      _entities = Entities.ProtoIt<TransformComponent, DestroyEvent>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _entities)
      {
        var transform = _transformAspect.TransformsPool.Get(entity).Transform;
        Object.Destroy(transform.gameObject);
        
        _destroyAspect.World().DelEntity(entity);
      }
    }
  }
}