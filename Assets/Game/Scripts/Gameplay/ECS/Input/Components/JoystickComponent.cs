using Game.Scripts.Infrastructure.Custom;
using JoystickPack.Scripts.Base;
using JoystickPack.Scripts.Joysticks;

namespace Game.Scripts.Gameplay.ECS.Input.Components
{
  public struct JoystickComponent
  {
    public Joystick AxisJoystick;
    public RotateJoystick RotateJoystick;
    public CustomButton JumpButton;
  }
}