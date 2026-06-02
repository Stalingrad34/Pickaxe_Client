using System;
using Game.Scripts.Gameplay.ECS.KinematicCharacter.Aspects;
using Game.Scripts.Gameplay.ECS.KinematicCharacter.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.KinematicCharacter
{
  public class KinematicCharacterModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      systems
        .AddSystem(new KinematicMoveSystem())
        .AddSystem(new KinematicJumpSystem())
        .AddSystem(new KinematicAnimationSystem());
    }

    public IProtoAspect[] Aspects()
    {
      return new IProtoAspect[]
      {
        new KinematicCharacterAspect()
      };
    }

    public Type[] Dependencies()
    {
      return null;
    }
  }
}