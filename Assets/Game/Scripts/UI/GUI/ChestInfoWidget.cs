using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.GUI
{
  public class ChestInfoWidget : WidgetView<ChestInfoModel>
  {
    [SerializeField] private CustomText chestName;
    [SerializeField] private CustomText cost;
    [SerializeField] private Image chestIcon;
    [SerializeField] private PickaxeVariantView pickaxeVariant;
    [SerializeField] private RectTransform pickaxeVariantsRoot;
    [SerializeField] private CustomButton open;
    [SerializeField] private CustomButton discard;
    
    protected override void SetModel(ChestInfoModel model)
    {
      
    }
  }
}