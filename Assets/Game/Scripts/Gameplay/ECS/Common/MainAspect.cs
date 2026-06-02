using System.Collections.Generic;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Common
{
  public sealed class MainAspect : IProtoAspect
  {
    public EventsAspect Events;
    
    private readonly List<IProtoAspect> _aspects = new ();
    private ProtoWorld _world;

    public void AddAspects(IProtoAspect[] aspect)
    {
      _aspects.AddRange(aspect);
    }
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      _world.AddAspect(this);
      
      foreach (var aspect in _aspects)
        aspect.Init(world);
      
      Events = new EventsAspect();
      Events.Init(world);
    }

    public void PostInit()
    {
      foreach (var aspect in _aspects)
        aspect.PostInit();
      
      Events.PostInit();
    }

    public ProtoWorld World()
    {
      return _world;
    }
  }
}
