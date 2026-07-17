using Game.Scripts.Gameplay.Ore;
using UnityEngine;

namespace Game.Scripts.Gameplay.Pickaxe
{
  [CreateAssetMenu(menuName = "Data/PickaxeConfig")]
  public class PickaxeConfig : ScriptableObject
  {
    public PickaxeView pickaxeView;
    public PickaxeType pickaxeType;
    public OreConfig oreConfig;
    public string nameKey;
    public Sprite available;
    public Sprite unavailable;
  }
}