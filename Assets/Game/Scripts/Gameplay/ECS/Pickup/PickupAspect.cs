using Game.Scripts.Gameplay.ECS.Pickup.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickup
{
  public class PickupAspect : IProtoAspect
  {
    public ProtoPool<PickupOreItemComponent> PickupOreItemsPool;
    public ProtoPool<PickupOreCollectorComponent> PickupOreCollectorsPool;

    private ProtoWorld _world;

    public void Init(ProtoWorld world)
    {
      _world = world;
      _world.AddAspect(this);

      PickupOreItemsPool = new ProtoPool<PickupOreItemComponent>();
      _world.AddPool(PickupOreItemsPool);
      
      PickupOreCollectorsPool = new ProtoPool<PickupOreCollectorComponent>();
      _world.AddPool(PickupOreCollectorsPool);
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