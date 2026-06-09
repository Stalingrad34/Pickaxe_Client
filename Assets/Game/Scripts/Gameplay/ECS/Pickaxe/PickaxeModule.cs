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
        .AddSystem(new SpawnPickaxeSystem())
        .AddSystem(new OneFrameSystem<SpawnPickaxeEvent>(aspect.SpawnPickaxeEvents));
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