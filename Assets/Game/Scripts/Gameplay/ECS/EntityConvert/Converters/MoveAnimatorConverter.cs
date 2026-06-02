using Game.Scripts.Gameplay.ECS.KinematicCharacter.Aspects;
using Game.Scripts.Gameplay.ECS.KinematicCharacter.Components;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Converters
{
  public class MoveAnimatorConverter : MonoBehaviour, IEntityConverter
  {
    [SerializeField] private CharacterAnimator animator;
    
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var move = systems.GetAspect<KinematicCharacterAspect>();
      ref var component = ref move.Animators.Add(entity);
      component.Animator = animator;
    }
  }
}