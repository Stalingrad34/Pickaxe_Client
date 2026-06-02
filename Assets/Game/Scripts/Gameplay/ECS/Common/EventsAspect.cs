using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Common
{
  public class EventsAspect : IProtoAspect
  {
    public ProtoPool<PlayerChangeEvent> PlayerChangeEvents;
    public ProtoIt PlayerChangeEventIt;

    public ProtoPool<OnCollisionEnterEvent> CollisionEnterEvents;
    public ProtoIt CollisionEnterEventIt;

    public ProtoPool<OnCollisionStayEvent> CollisionStayEvents;
    public ProtoIt CollisionStayEventIt;

    public ProtoPool<OnCollisionExitEvent> CollisionExitEvents;
    public ProtoIt CollisionExitEventIt;
    
    private ProtoWorld _world;

    public void Init(ProtoWorld world)
    {
      _world = world;
      world.AddAspect(this);

      PlayerChangeEvents = new ProtoPool<PlayerChangeEvent>();
      CollisionEnterEvents = new ProtoPool<OnCollisionEnterEvent>();
      CollisionStayEvents = new ProtoPool<OnCollisionStayEvent>();
      CollisionExitEvents = new ProtoPool<OnCollisionExitEvent>();

      world.AddPool(PlayerChangeEvents);
      world.AddPool(CollisionEnterEvents);
      world.AddPool(CollisionStayEvents);
      world.AddPool(CollisionExitEvents);
    }

    public void PostInit()
    {
      PlayerChangeEventIt = new ProtoIt(new[] { typeof(PlayerChangeEvent) });
      PlayerChangeEventIt.Init(_world);

      CollisionEnterEventIt = new ProtoIt(new[] { typeof(OnCollisionEnterEvent) });
      CollisionEnterEventIt.Init(_world);

      CollisionStayEventIt = new ProtoIt(new[] { typeof(OnCollisionStayEvent) });
      CollisionStayEventIt.Init(_world);

      CollisionExitEventIt = new ProtoIt(new[] { typeof(OnCollisionExitEvent) });
      CollisionExitEventIt.Init(_world);
    }

    public ProtoWorld World() => _world;
  }
}