using Game.Scripts.Gameplay.ECS.Camera.Components;
using Game.Scripts.Gameplay.ECS.Input.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Camera.Aspects
{
  public class CameraAspect : IProtoAspect
  {
    public ProtoPool<CameraComponent> Cameras;
    public ProtoIt Entities;
    private ProtoWorld _world;
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      _world.AddAspect(this);
      
      Cameras = new ProtoPool<CameraComponent>();
      _world.AddPool(Cameras);
    }

    public void PostInit()
    {
      Entities = new ProtoIt(new [] {typeof(ControlComponent), typeof(CameraComponent)});
      Entities.Init(_world);
    }

    public ProtoWorld World()
    {
      return _world;
    }
  }
}