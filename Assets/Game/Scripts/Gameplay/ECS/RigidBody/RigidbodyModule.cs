using System;
using Game.Scripts.Gameplay.ECS.RigidBody.Aspects;
using Game.Scripts.Gameplay.ECS.RigidBody.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.RigidBody
{
  public class RigidbodyModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      systems
        .AddSystem(new ForceSystem());
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