using Game.Scripts.Gameplay.ECS.Destroy.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Destroy
{
  public class DestroyAspect : IProtoAspect
  {
    public ProtoPool<DestroyEvent> DestroyPool;

    private ProtoWorld _world;

    public void Init(ProtoWorld world)
    {
      _world = world;
      _world.AddAspect(this);

      DestroyPool = new ProtoPool<DestroyEvent>();
      _world.AddPool(DestroyPool);
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