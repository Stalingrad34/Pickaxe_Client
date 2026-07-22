using System;
using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.Pickup.Components;
using Game.Scripts.Gameplay.ECS.Pickup.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickup
{
  public class PickupModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      var aspect = systems.GetAspect<PickupAspect>();
      systems
        .AddSystem(new PickupSystem())
        .AddSystem(new PickupOreSystem())
        .AddSystem(new PickupChestSystem())
        .AddSystem(new OneFrameSystem<PickupEvent>(aspect.PickupEventsPool));
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