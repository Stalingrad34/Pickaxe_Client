using Game.Scripts.Gameplay.ECS.Camera.Aspects;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Camera.Systems
{
  public class UpdateCameraSystem : IProtoInitSystem, IProtoRunSystem
  {
    private CameraAspect _aspect;
    
    public void Init(IProtoSystems systems)
    {
      _aspect = systems.GetAspect<CameraAspect>();
    }

    public void Run()
    {
      foreach (var entity in _aspect.Entities)
      {
        var camera = _aspect.Cameras.Get(entity);
        camera.Camera.UpdateWithInput(Time.deltaTime, camera.ScrollInput, camera.LookInput);
      }
    }
  }
}