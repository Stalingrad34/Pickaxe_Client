using UnityEngine;

namespace Game.Scripts.Gameplay.Pickaxe
{
  public class PickaxeView : MonoBehaviour
  {
    private PickaxeConfig _pickaxeConfig;

    public void Init(PickaxeConfig pickaxeConfig)
    {
      _pickaxeConfig = pickaxeConfig;
    }
    
    public PickaxeType GetPickaxeType()
    {
      return _pickaxeConfig.pickaxeType;
    }
  }
}