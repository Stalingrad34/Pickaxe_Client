using Game.Scripts.Gameplay.ECS.Input.Aspects;
using Game.Scripts.Gameplay.ECS.Input.Components;
using Leopotam.EcsProto;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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
          control.MouseScroll = GetTouchZoom();
          control.SpaceKeyDown = joystick.JumpButton.WasPressed;
        }
      }
    }

    private float GetTouchZoom()
    {
      var touchscreen = Touchscreen.current;
      if (touchscreen == null)
        return 0f;

      TouchControl firstTouch = null;
      TouchControl secondTouch = null;

      foreach (var touch in touchscreen.touches)
      {
        if (!touch.press.isPressed)
          continue;

        if (firstTouch == null)
          firstTouch = touch;
        else
        {
          secondTouch = touch;
          break;
        }
      }

      if (firstTouch == null || secondTouch == null)
        return 0f;

      var firstPosition = firstTouch.position.ReadValue();
      var secondPosition = secondTouch.position.ReadValue();

      var firstPreviousPosition = firstPosition - firstTouch.delta.ReadValue();
      var secondPreviousPosition = secondPosition - secondTouch.delta.ReadValue();

      var currentDistance = Vector2.Distance(firstPosition, secondPosition);
      var previousDistance = Vector2.Distance(firstPreviousPosition, secondPreviousPosition);

      var distanceDelta = currentDistance - previousDistance;

      if (Mathf.Abs(distanceDelta) < 2)
        return 0f;

      return distanceDelta > 0f ? -1f : 1f;
    }
  }
}