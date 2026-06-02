using Game.Scripts.Gameplay.ECS.RigidBody.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.RigidBody.Aspects
{
  public class RigidbodyAspect : IProtoAspect
  {
    public ProtoPool<AddForceEvent> AddForceEvents;
    public ProtoPool<RigidbodyComponent> Rigidbodies;
    private ProtoWorld _world;
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      world.AddAspect(this);
      
      AddForceEvents = new ProtoPool<AddForceEvent>();
      world.AddPool(AddForceEvents);
      
      Rigidbodies = new ProtoPool<RigidbodyComponent>();
      world.AddPool(Rigidbodies);
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