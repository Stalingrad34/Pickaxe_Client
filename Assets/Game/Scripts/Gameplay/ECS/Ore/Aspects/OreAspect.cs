using Game.Scripts.Gameplay.ECS.Ore.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Ore.Aspects
{
  public class OreAspect : IProtoAspect
  {
    public ProtoPool<OreComponent> OrePool;
    public ProtoPool<OreSpawnEvent> OreSpawnPool;
    private ProtoWorld _world;
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      world.AddAspect(this);
      
      OrePool = new ProtoPool<OreComponent>();
      world.AddPool(OrePool);
      
      OreSpawnPool = new ProtoPool<OreSpawnEvent>();
      world.AddPool(OreSpawnPool);
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