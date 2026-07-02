using Game.Scripts.Gameplay.ECS.Character.Components;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Character.Aspects
{
  public class CharacterAspect : IProtoAspect
  {
    public ProtoPool<CharacterComponent> CharacterComponentPool;
    public ProtoPool<CharacterSpawnEvent> CharacterSpawnPool;
    public ProtoPool<CharacterPickupComponent> CharacterPickupPool;
    private ProtoWorld _world;
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      world.AddAspect(this);
      
      CharacterComponentPool = new ProtoPool<CharacterComponent>();
      world.AddPool(CharacterComponentPool);
      
      CharacterSpawnPool = new ProtoPool<CharacterSpawnEvent>();
      world.AddPool(CharacterSpawnPool);
      
      CharacterPickupPool = new ProtoPool<CharacterPickupComponent>();
      world.AddPool(CharacterPickupPool);
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