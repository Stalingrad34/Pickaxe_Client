using System;
using Game.Scripts.Gameplay.ECS.Destroy.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Destroy
{
  public class DestroyModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      systems
        .AddSystem(new DestroyGameObjectSystem());
    }

    public IProtoAspect[] Aspects()
    {
      return new IProtoAspect[]
      {
        new DestroyAspect()
      };
    }

    public Type[] Dependencies()
    {
      return null;
    }
  }
}