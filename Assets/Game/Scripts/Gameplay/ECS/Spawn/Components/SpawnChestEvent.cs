using Game.Scripts.Gameplay.Chest;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Spawn.Components
{
  public struct SpawnChestEvent
  {
    public Vector3 Position;
    public Vector3 Direction;
    public ChestConfig Config;
  }
}