using Game.Scripts.Gameplay.ECS.Pickaxe.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickaxe.Aspects
{
  public class PickaxeAspect : IProtoAspect
  {
    public ProtoPool<PickaxeComponent> Pickaxes;
    public ProtoPool<PickaxeMineComponent> PickaxeMines;
    public ProtoPool<RebuildPickaxeEvent> RebuildPickaxeEvents;
    private ProtoWorld _world;
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      _world.AddAspect(this);
      
      Pickaxes = new ProtoPool<PickaxeComponent>();
      _world.AddPool(Pickaxes);
      
      PickaxeMines = new ProtoPool<PickaxeMineComponent>();
      _world.AddPool(PickaxeMines);
      
      RebuildPickaxeEvents = new ProtoPool<RebuildPickaxeEvent>();
      _world.AddPool(RebuildPickaxeEvents);
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