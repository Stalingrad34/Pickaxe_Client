namespace Game.Scripts.Gameplay.ECS.Pickup.Interfaces
{
  public interface IPickupCollector
  {
    bool CanTake();
    void Take(IPickupItem item);
    void Discard();
  }
}