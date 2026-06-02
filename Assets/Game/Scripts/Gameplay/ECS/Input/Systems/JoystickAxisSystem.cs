using Game.Scripts.Gameplay.ECS.Input.Aspects;
using Game.Scripts.Gameplay.ECS.Input.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Input.Systems
{
  public class JoystickAxisSystem : IProtoInitSystem, IProtoRunSystem
  {
    private InputAspect _input;
    private ProtoIt _joystickEntities;
    private ProtoIt _controlEntities;
    
    public void Init(IProtoSystems systems)
    {
      _input = systems.GetAspect<InputAspect>();
      _joystickEntities = Entities.ProtoIt<JoystickComponent>(systems.World());
      _controlEntities = Entities.ProtoIt<ControlComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var joystickEntity in _joystickEntities)
      {
        ref var joystick = ref _input.Joysticks.Get(joystickEntity);
        
        foreach (var entity in _controlEntities)
        {
          ref var control = ref _input.Controls.Get(entity);
          if (control.IsKeysLocked) 
            continue;
          
          control.HorizontalAxis = joystick.AxisJoystick.Horizontal;
          control.VerticalAxis = joystick.AxisJoystick.Vertical;
        }
      }
    }
  }
}