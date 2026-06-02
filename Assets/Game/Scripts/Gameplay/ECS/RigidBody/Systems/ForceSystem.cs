using Game.Scripts.Gameplay.ECS.RigidBody.Aspects;
using Game.Scripts.Gameplay.ECS.RigidBody.Components;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.RigidBody.Systems
{
  public class ForceSystem : IProtoInitSystem, IProtoRunSystem
  {
    private RigidbodyAspect _aspect;
    private ProtoIt _entities;
    
    public void Init(IProtoSystems systems)
    {
      _aspect = systems.GetAspect<RigidbodyAspect>();
      _entities = Entities.ProtoIt<RigidbodyComponent, AddForceEvent>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _entities)
      {
        var rigidbody = _aspect.Rigidbodies.Get(entity).Rigidbody;
        rigidbody.AddForce(_aspect.AddForceEvents.Get(entity).Force, ForceMode.VelocityChange);
      }
    }
  }
}