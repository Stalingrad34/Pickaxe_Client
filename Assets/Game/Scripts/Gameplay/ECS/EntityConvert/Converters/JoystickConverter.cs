using Game.Scripts.Gameplay.ECS.Input.Aspects;
using Game.Scripts.Infrastructure.Custom;
using JoystickPack.Scripts.Base;
using JoystickPack.Scripts.Joysticks;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Converters
{
  public class JoystickConverter : MonoBehaviour, IEntityConverter
  {
    [SerializeField] private Joystick axisJoystick;
    [SerializeField] private RotateJoystick rotateJoystick;
    [SerializeField] private CustomButton jumpButton;
    
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      ref var component = ref systems.GetAspect<InputAspect>().Joysticks.Add(entity);
      component.AxisJoystick = axisJoystick;
      component.RotateJoystick = rotateJoystick;
      component.JumpButton = jumpButton;
    }
  }
}