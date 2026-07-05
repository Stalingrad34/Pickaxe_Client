using Game.Scripts.Gameplay.ECS.Common.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Common
{
  public class TransformAspect : IProtoAspect
  {
    public ProtoPool<TransformComponent> TransformsPool;
    private ProtoWorld _world;

    public void Init(ProtoWorld world)
    {
      _world = world;
      _world.AddAspect(this);
      
      TransformsPool = new ProtoPool<TransformComponent>();
      _world.AddPool(TransformsPool);
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