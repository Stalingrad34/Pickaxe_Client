using System;
using Game.Scripts.Gameplay.ECS.Character.Aspects;
using Game.Scripts.Gameplay.ECS.Character.Systems;
using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Character
{
  public class CharacterModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      var aspect = systems.GetAspect<CharacterAspect>();
      systems
        .AddSystem(new CharacterSpawnSystem())
        .AddSystem(new OneFrameSystem<CharacterSpawnEvent>(aspect.CharacterSpawnPool));
    }

    public IProtoAspect[] Aspects()
    {
      return new IProtoAspect[]
      {
        new CharacterAspect()
      };
    }

    public Type[] Dependencies()
    {
      return null;
    }
  }
}