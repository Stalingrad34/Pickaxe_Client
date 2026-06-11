using System;
using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.Pickaxe.Aspects;
using Game.Scripts.Gameplay.ECS.Pickaxe.Components;
using Game.Scripts.Gameplay.ECS.Pickaxe.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickaxe
{
  public class PickaxeModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      var aspect = systems.GetAspect<PickaxeAspect>();
      systems
        .AddSystem(new RebuildPickaxesSystem())
        .AddSystem(new PickaxesPunchSystem())
        .AddSystem(new OneFrameSystem<RebuildPickaxeEvent>(aspect.RebuildPickaxeEvents))
        .AddSystem(new OneFrameSystem<PickaxesPunchEvent>(aspect.PickaxesPunchEvents));
    }

    public IProtoAspect[] Aspects()
    {
      return new IProtoAspect[]
      {
        new PickaxeAspect()
      };
    }

    public Type[] Dependencies()
    {
      return null;
    }
  }
}