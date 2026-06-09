using UnityEngine;

namespace Game.Scripts.Gameplay.Units
{
  [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Player Data")]
  public class PlayerData : ScriptableObject
  {
    public float Speed;
  }
}