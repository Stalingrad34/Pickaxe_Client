using Game.Scripts.Gameplay.ECS.Pickaxe.Components;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Spawn.Aspects
{
  public class SpawnAspect : IProtoAspect
  {
    public ProtoPool<SpawnCharacterEvent> SpawnCharacterEvents;
    private ProtoWorld _world;
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      world.AddAspect(this);
      
      SpawnCharacterEvents = new ProtoPool<SpawnCharacterEvent>();
      world.AddPool(SpawnCharacterEvents);
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