using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Infrastructure.Custom;
using UnityEngine;

namespace Game.Scripts.UI.GUI
{
  public class PickaxeVariantView : MonoBehaviour
  {
    [SerializeField] private CustomText pickaxeName;
    [SerializeField] private CustomText pickaxeTier;
    [SerializeField] private CustomText percent;

    public void Init(PickaxeVariant variant)
    {
      pickaxeName.SetText(variant.pickaxeConfig.nameKey);
      pickaxeName.color = variant.pickaxeConfig.oreConfig.pickupColor;
      
      var level = (int) variant.pickaxeConfig.pickaxeType;
      pickaxeTier.SetText(new TextData("pickaxe_level", level.ToString()));
      
      percent.text = $"{variant.Chance}%";
    }
  }
}