using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.Input.Aspects;
using Game.Scripts.Gameplay.ECS.Input.Components;
using Game.Scripts.Gameplay.ECS.KinematicCharacter.Aspects;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.KinematicCharacter.Systems
{
  public class KinematicAnimationSystem : IProtoInitSystem, IProtoRunSystem
  {
    private KinematicCharacterAspect _character;
    private InputAspect _input;
    private ProtoIt _entities;
    
    public void Init(IProtoSystems systems)
    {
      _character = systems.GetAspect<KinematicCharacterAspect>();
      _input = systems.GetAspect<InputAspect>();
      _entities = Entities.ProtoIt<ControlComponent, CharacterAnimatorComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _entities)
      {
        var horizontal = _input.Controls.Get(entity).HorizontalAxis;
        var vertical = _input.Controls.Get(entity).VerticalAxis;
        var animator = _character.Animators.Get(entity).Animator;
        
        animator.SetMoveAnimation(!Mathf.Approximately(horizontal, 0) || !Mathf.Approximately(vertical, 0));
        animator.SetFlyAnimation(!_character.Kinematics.Get(entity).Processor.Motor.GroundingStatus.FoundAnyGround);
      }
    }
  }
}