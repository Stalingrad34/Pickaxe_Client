using Game.Scripts.Gameplay.Ore;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Ore.Components
{
  public struct OreSpawnEvent
  {
    public Vector3 Position;
    public Vector3 Direction;
    public OreConfig Config;
  }
}