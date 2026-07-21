using System;
using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Game.Scripts.Gameplay.ECS.Spawn.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Spawn
{
  public class SpawnModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      var aspect = systems.GetAspect<SpawnAspect>();
      systems
        .AddSystem(new SpawnCharacterSystem())
        .AddSystem(new SpawnOreSystem())
        .AddSystem(new SpawnChestSystem())
        .AddSystem(new OneFrameSystem<SpawnCharacterEvent>(aspect.SpawnCharacterPool))
        .AddSystem(new OneFrameSystem<SpawnOreEvent>(aspect.SpawnOrePool))
        .AddSystem(new OneFrameSystem<SpawnChestEvent>(aspect.SpawnChestPool));
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