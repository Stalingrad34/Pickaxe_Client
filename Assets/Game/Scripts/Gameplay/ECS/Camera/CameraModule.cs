using System;
using Game.Scripts.Gameplay.ECS.Camera.Aspects;
using Game.Scripts.Gameplay.ECS.Camera.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Camera
{
  public class CameraModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      systems
        .AddSystem(new RotateCameraSystem())
        .AddSystem(new UpdateCameraSystem());
    }

    public IProtoAspect[] Aspects()
    {
      return new IProtoAspect[]
      {
        new CameraAspect()
      };
    }

    public Type[] Dependencies()
    {
      return null;
    }
  }
}