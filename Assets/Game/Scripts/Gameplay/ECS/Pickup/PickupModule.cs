using System;
using Game.Scripts.Gameplay.ECS.Pickup.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickup
{
  public class PickupModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      systems
        .AddSystem(new PickupOreSystem());
    }

    public IProtoAspect[] Aspects()
    {
      return new IProtoAspect[]
      {
        new PickupAspect()
      };
    }

    public Type[] Dependencies()
    {
      return null;
    }
  }
}