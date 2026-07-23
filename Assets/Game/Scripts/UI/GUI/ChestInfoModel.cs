using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Gameplay.ECS.Pickup.Interfaces;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Widgets;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;

namespace Game.Scripts.UI.GUI
{
  public class ChestInfoModel : WidgetModel
  {
    public readonly ReactiveProperty<TextData> ChestName = new ();
    public readonly ReactiveProperty<Sprite> ChestIcon = new ();
    public readonly ReactiveProperty<Color> BackgroundColor = new ();
    public readonly ReactiveCollection<PickaxeVariant> PickaxeVariants = new ();
    public readonly ReactiveProperty<long> Cost = new ();
    
    private readonly IPickupCollector _collector;
    private readonly MainGUIModel _mainGUI;

    public ChestInfoModel(IPickupCollector collector, ChestConfig chestConfig, MainGUIModel mainGUI)
    {
      _collector = collector;
      _mainGUI = mainGUI;
      ChestName.Value = new TextData(chestConfig.ChestName);
      ChestIcon.Value = chestConfig.ChestIcon;
      BackgroundColor.Value = chestConfig.Color;
      PickaxeVariants.AddRange(chestConfig.Variants);
    }

    public void Open()
    {
      _mainGUI.HideChestInfo();
    }

    public void Discard()
    {
      _collector.Discard();
      _mainGUI.HideChestInfo();
    }
  }
}