using Game.Scripts.Gameplay.Pickaxe;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Pickaxe.Components
{
  public struct PickaxeMineComponent
  {
    public string OwnerId;
    public PickaxesHolder PickaxesHolder;
    public Transform PlayerSpawnPosition;
  }
}