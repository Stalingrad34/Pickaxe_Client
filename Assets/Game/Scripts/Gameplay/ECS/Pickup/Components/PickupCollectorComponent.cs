using Game.Scripts.Gameplay.ECS.Pickup.Interfaces;

namespace Game.Scripts.Gameplay.ECS.Pickup.Components
{
  public struct PickupCollectorComponent
  {
    public float PickupDistance;
    public IPickupCollector PickupCollector;
  }
}