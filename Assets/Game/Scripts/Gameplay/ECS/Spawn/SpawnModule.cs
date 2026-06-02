using System;
using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.Spawn.Aspects;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Game.Scripts.Gameplay.ECS.Spawn.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Spawn
{
  public class SpawnModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      var spawnAspect = systems.GetAspect<SpawnAspect>();
      systems
        .AddSystem(new SpawnCharacterSystem())
        .AddSystem(new OneFrameSystem<SpawnCharacterEvent>(spawnAspect.SpawnEvents));
    }

    public IProtoAspect[] Aspects()
    {
      return new IProtoAspect[]
      {
        new SpawnAspect()
      };
    }

    public Type[] Dependencies()
    {
      return null;
    }
  }
}