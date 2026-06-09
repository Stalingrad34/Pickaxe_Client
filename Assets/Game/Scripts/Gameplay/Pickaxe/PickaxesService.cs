using UnityEngine;

namespace Game.Scripts.Gameplay.Pickaxe
{
  public class PickaxesService
  {
    public int GetPickaxeCost(int pickaxeCount)
    {
      return Mathf.FloorToInt(1.4f * pickaxeCount + 6.5f);
    }
  }
}