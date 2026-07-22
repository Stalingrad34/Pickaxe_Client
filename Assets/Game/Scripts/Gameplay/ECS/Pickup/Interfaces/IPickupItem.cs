using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Pickup.Interfaces
{
  public interface IPickupItem
  {
    public Transform  Transform { get; }
    void Pickup();
  }
}