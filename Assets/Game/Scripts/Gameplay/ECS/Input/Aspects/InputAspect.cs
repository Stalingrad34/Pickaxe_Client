using Game.Scripts.Gameplay.ECS.Input.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Input.Aspects
{
  public class InputAspect : IProtoAspect
  {
    public ProtoPool<ControlComponent> Controls;
    public ProtoPool<JoystickComponent> Joysticks;
    private ProtoWorld _world;
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      world.AddAspect(this);
      
      Controls = new ProtoPool<ControlComponent>();
      world.AddPool(Controls);
      
      Joysticks = new ProtoPool<JoystickComponent>();
      world.AddPool(Joysticks);
    }

    public void PostInit()
    {
    }

    public ProtoWorld World()
    {
      return _world;
    }
  }
}