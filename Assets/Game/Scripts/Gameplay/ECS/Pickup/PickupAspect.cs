using Game.Scripts.Gameplay.ECS.Pickup.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Pickup
{
  public class PickupAspect : IProtoAspect
  {
    public ProtoPool<PickupCollectorComponent> PickupCollectorsPool;
    public ProtoPool<PickupItemComponent> PickupItemsPool;
    public ProtoPool<OreRewardComponent> PickupOreRewardsPool;
    public ProtoPool<PickupEvent> PickupEventsPool;

    private ProtoWorld _world;

    public void Init(ProtoWorld world)
    {
      _world = world;
      _world.AddAspect(this);
      
      PickupCollectorsPool = new ProtoPool<PickupCollectorComponent>();
      _world.AddPool(PickupCollectorsPool);

      PickupItemsPool = new ProtoPool<PickupItemComponent>();
      _world.AddPool(PickupItemsPool);
      
      PickupOreRewardsPool = new ProtoPool<OreRewardComponent>();
      _world.AddPool(PickupOreRewardsPool);
      
      PickupEventsPool = new ProtoPool<PickupEvent>();
      _world.AddPool(PickupEventsPool);
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