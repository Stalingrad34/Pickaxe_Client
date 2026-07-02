using System;
using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.RigidBody.Aspects;
using Game.Scripts.Gameplay.ECS.RigidBody.Components;
using Game.Scripts.Gameplay.ECS.RigidBody.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.RigidBody
{
  public class RigidbodyModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      var aspect = systems.GetAspect<RigidbodyAspect>();
      systems
        .AddSystem(new ForceSystem())
        .AddSystem(new OneFrameSystem<AddForceEvent>(aspect.AddForceEvents));
    }

    public IProtoAspect[] Aspects()
    {
      return new IProtoAspect[]
      {
        new RigidbodyAspect()
      };
    }

    public Type[] Dependencies()
    {
      return null;
    }
  }
}