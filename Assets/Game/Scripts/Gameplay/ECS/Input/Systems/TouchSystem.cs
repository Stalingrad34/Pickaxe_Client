using Game.Scripts.Gameplay.ECS.Input.Aspects;
using Game.Scripts.Gameplay.ECS.Input.Components;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Input.Systems
{
  public class TouchSystem : IProtoInitSystem, IProtoRunSystem
  {
    private InputAspect _input;
    private InputActions _inputAction;
    private ProtoIt _joystickEntities;
    private ProtoIt _controlEntities;

    public void Init(IProtoSystems systems)
    {
      _input = systems.GetAspect<InputAspect>();
      _inputAction = systems.GetService<InputActions>();
      _controlEntities = Entities.ProtoIt<ControlComponent>(systems.World());
      _joystickEntities = Entities.ProtoIt<JoystickComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var joystickEntity in _joystickEntities)
      {
        var joystick = _input.Joysticks.Get(joystickEntity);
        
        foreach (var entity in _controlEntities)
        {
          ref var control = ref _input.Controls.Get(entity);
          if (control.IsTouchLocked)
            continue;
          
          var sensitivity = 1;//ServiceProvider.Get<SettingsProvider>().SensitivityValue.Value;
          control.MouseHorizontal = joystick.RotateJoystick.delta.x * sensitivity;
          control.MouseVertical = joystick.RotateJoystick.delta.y * sensitivity;
          control.MouseScroll = -_inputAction.Player.Zoom.ReadValue<Vector2>().y;
          control.MouseLeftClicked = joystick.ActionButton.WasPressed;
        }
      }
    }
  }
}