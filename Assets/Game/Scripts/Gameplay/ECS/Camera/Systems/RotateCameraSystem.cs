using System;
using Game.Scripts.Gameplay.ECS.Camera.Aspects;
using Game.Scripts.Gameplay.ECS.Camera.Components;
using Game.Scripts.Gameplay.ECS.Input.Aspects;
using Game.Scripts.Gameplay.ECS.Input.Components;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Camera.Systems
{
  public class RotateCameraSystem : IProtoInitSystem, IProtoRunSystem
  {
    private InputAspect _input;
    private CameraAspect _camera;
    private ProtoIt _entities;
    
    public void Init(IProtoSystems systems)
    {
      _input = systems.GetAspect<InputAspect>();
      _camera = systems.GetAspect<CameraAspect>();
      
      _entities = Entities.ProtoIt<ControlComponent, CameraComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _entities)
      {
        var control = _input.Controls.Get(entity);
        _camera.Cameras.Get(entity).LookInput = new Vector3(control.MouseHorizontal, control.MouseVertical, 0);
      }
    }
  }
}