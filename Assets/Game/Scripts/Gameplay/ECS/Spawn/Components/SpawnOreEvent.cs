using Game.Scripts.Gameplay.Ore;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Spawn.Components
{
  public struct SpawnOreEvent
  {
    public Vector3 Position;
    public Vector3 Direction;
    public OreConfig Config;
  }
}