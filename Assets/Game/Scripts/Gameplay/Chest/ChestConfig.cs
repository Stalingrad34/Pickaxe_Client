using System;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Pickaxe;
using UnityEngine;

namespace Game.Scripts.Gameplay.Chest
{
  [CreateAssetMenu(menuName = "Data/ChestConfig")]
  public class ChestConfig : ScriptableObject
  {
    public ChestView Prefab;
    public string ChestName;
    public Sprite ChestIcon;
    public int Weight;
    public List<PickaxeVariant> Variants;
  }

  [Serializable]
  public class PickaxeVariant
  {
    public PickaxeConfig pickaxeConfig;
    public int Chance;
  }
}