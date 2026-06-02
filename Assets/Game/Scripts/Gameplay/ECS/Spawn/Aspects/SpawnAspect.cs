using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Spawn.Aspects
{
  public class SpawnAspect : IProtoAspect
  {
    public ProtoPool<SpawnCharacterEvent> SpawnEvents;
    private ProtoWorld _world;
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      world.AddAspect(this);
      
      SpawnEvents = new ProtoPool<SpawnCharacterEvent>();
      world.AddPool(SpawnEvents);
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