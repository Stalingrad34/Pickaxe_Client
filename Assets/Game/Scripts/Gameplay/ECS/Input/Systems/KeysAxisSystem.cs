using Game.Scripts.Gameplay.ECS.Input.Aspects;
using Game.Scripts.Gameplay.ECS.Input.Components;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Input.Systems
{
  public class KeysAxisSystem : IProtoInitSystem, IProtoRunSystem
  {
    private InputAspect _input;
    private InputActions _inputAction;
    private ProtoIt _entities;

    public void Init(IProtoSystems systems)
    {
      _input = systems.GetAspect<InputAspect>();
      _inputAction = systems.GetService<InputActions>();
      _entities = Entities.ProtoIt<ControlComponent>(systems.World());
    }

    public void Run()
    {
      var horizontal = _inputAction.Player.Move.ReadValue<Vector2>().x;
      var vertical = _inputAction.Player.Move.ReadValue<Vector2>().y;
      var isJump = _inputAction.Player.Jump.triggered;
      
      foreach (var entity in _entities)
      {
        ref var control = ref _input.Controls.Get(entity);
        if (control.IsKeysLocked)
          continue;
        
        control.HorizontalAxis = horizontal;
        control.VerticalAxis = vertical;
        control.SpaceKeyDown = isJump;
      }
    }
  }
}