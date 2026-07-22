using Game.Scripts.Gameplay.ECS.Destroy.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Destroy
{
  public class DestroyAspect : IProtoAspect
  {
    public ProtoPool<DestroyEntityEvent> DestroyEntitiesPool;
    public ProtoPool<DestroyGameObjectEvent> DestroyGameObjectsPool;

    private ProtoWorld _world;

    public void Init(ProtoWorld world)
    {
      _world = world;
      _world.AddAspect(this);

      DestroyEntitiesPool = new ProtoPool<DestroyEntityEvent>();
      _world.AddPool(DestroyEntitiesPool);
      
      DestroyGameObjectsPool = new ProtoPool<DestroyGameObjectEvent>();
      _world.AddPool(DestroyGameObjectsPool);
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