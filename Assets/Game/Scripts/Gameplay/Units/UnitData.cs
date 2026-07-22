using Game.Scripts.Gameplay.ECS.Pickup.Interfaces;
using UnityEngine;

namespace Game.Scripts.Gameplay.Units
{
  public class UnitData
  {
    public string Id;
    public string PlayerName;
    public float StartAngleY;
    public Vector3 Position;
    public IPickupCollector Collector;
  }
}