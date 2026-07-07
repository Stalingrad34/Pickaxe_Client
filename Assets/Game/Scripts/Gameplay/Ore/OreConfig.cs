using UnityEngine;

namespace Game.Scripts.Gameplay.Ore
{
  [CreateAssetMenu(menuName = "Data/OreConfig")]
  public class OreConfig : ScriptableObject
  {
    public int miningCount;
    public OreView prefab;
    public Color pickupColor = Color.white;
  }
}