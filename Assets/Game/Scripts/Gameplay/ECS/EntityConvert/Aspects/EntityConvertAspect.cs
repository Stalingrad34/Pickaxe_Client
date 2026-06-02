using Game.Scripts.Gameplay.ECS.EntityConvert.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Aspects
{
  public class EntityConvertAspect : IProtoAspect
  {
    public ProtoPool<EntityConvertEvent> Events;
    private ProtoWorld _world;
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      world.AddAspect(this);
      
      Events = new ProtoPool<EntityConvertEvent>();
      world.AddPool(Events);
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