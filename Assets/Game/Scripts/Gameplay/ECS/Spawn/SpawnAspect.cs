using Game.Scripts.Gameplay.ECS.Common.Components;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Spawn
{
  public class SpawnAspect : IProtoAspect
  {
    public ProtoPool<SpawnCharacterEvent> SpawnCharacterPool;
    public ProtoPool<SpawnOreEvent> SpawnOrePool;
    public ProtoPool<SpawnChestEvent> SpawnChestPool;

    public ProtoPool<GameSessionComponent> GameSessionPool;
    public ProtoPool<CharacterComponent> CharacterPool;
    public ProtoPool<OreComponent> OrePool;
    public ProtoPool<ChestComponent> ChestPool;

    private ProtoWorld _world;

    public void Init(ProtoWorld world)
    {
      _world = world;
      _world.AddAspect(this);
      
      GameSessionPool = new ProtoPool<GameSessionComponent>();
      _world.AddPool(GameSessionPool);

      SpawnCharacterPool = new ProtoPool<SpawnCharacterEvent>();
      _world.AddPool(SpawnCharacterPool);
      
      SpawnOrePool = new ProtoPool<SpawnOreEvent>();
      _world.AddPool(SpawnOrePool);
      
      SpawnChestPool = new ProtoPool<SpawnChestEvent>();
      _world.AddPool(SpawnChestPool);
      
      CharacterPool = new ProtoPool<CharacterComponent>();
      _world.AddPool(CharacterPool);
      
      OrePool = new ProtoPool<OreComponent>();
      _world.AddPool(OrePool);
      
      ChestPool = new ProtoPool<ChestComponent>();
      _world.AddPool(ChestPool);
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