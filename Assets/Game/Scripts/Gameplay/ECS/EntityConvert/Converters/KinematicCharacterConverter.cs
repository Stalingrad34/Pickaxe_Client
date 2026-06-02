using Game.Scripts.Gameplay.ECS.KinematicCharacter.Aspects;
using Game.Scripts.KinematicCharacterController.Core;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Converters
{
  public class KinematicCharacterConverter : MonoBehaviour, IEntityConverter
  {
    [SerializeField] private KinematicCharacterProcessor processor;
    
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var moveAspect = systems.GetAspect<KinematicCharacterAspect>();
      ref var component = ref moveAspect.Kinematics.Add(entity);
      component.Processor = processor;
    }
  }
}