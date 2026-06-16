using System;
using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.Ore.Aspects;
using Game.Scripts.Gameplay.ECS.Ore.Components;
using Game.Scripts.Gameplay.ECS.Ore.Systems;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Ore
{
  public class OreModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      var oreAspect = systems.GetAspect<OreAspect>();
      systems
        .AddSystem(new OreSpawnSystem())
        .AddSystem(new OneFrameSystem<OreSpawnEvent>(oreAspect.OreSpawnPool));
    }

    public IProtoAspect[] Aspects()
    {
      return new IProtoAspect[] { new OreAspect()};
    }

    public Type[] Dependencies()
    {
      return null;
    }
  }
}