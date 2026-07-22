using Game.Scripts.Gameplay.ECS.Destroy.Components;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Destroy.Systems
{
  public class DestroyGameObjectSystem : IProtoInitSystem, IProtoRunSystem
  {
    private DestroyAspect _destroyAspect;
    private ProtoIt _entities;

    public void Init(IProtoSystems systems)
    {
      _destroyAspect = systems.GetAspect<DestroyAspect>();
      _entities = Entities.ProtoIt<DestroyGameObjectEvent>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _entities)
      {
        var gameObject = _destroyAspect.DestroyGameObjectsPool.Get(entity).GameObject;
        Object.Destroy(gameObject);
        
        _destroyAspect.World().DelEntity(entity);
      }
    }
  }
}