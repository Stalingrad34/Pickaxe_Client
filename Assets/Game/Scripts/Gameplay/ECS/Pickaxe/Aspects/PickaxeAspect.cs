using Game.Scripts.Gameplay.ECS.Pickaxe.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickaxe.Aspects
{
  public class PickaxeAspect : IProtoAspect
  {
    public ProtoPool<SpawnPickaxeEvent> SpawnPickaxeEvents;
    private ProtoWorld _world;
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      _world.AddAspect(this);
      
      SpawnPickaxeEvents = new ProtoPool<SpawnPickaxeEvent>();
      _world.AddPool(SpawnPickaxeEvents);
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