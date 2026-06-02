using Game.Scripts.Gameplay.ECS.Camera.Aspects;
using Game.Scripts.KinematicCharacterController.ExampleCharacter.Scripts;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Converters
{
  public class CameraConverter : MonoBehaviour, IEntityConverter
  {
    [SerializeField] private ExampleCharacterCamera characterCamera;
    
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<CameraAspect>();
      ref var component = ref aspect.Cameras.Add(entity);
      component.Camera = characterCamera;
    }
  }
}