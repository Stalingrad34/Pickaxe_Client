using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Gameplay.ECS.Pickup.Interfaces;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.InApp;
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
    public readonly ReactiveProperty<bool> ShowInApp = new ();
    public readonly ReactiveProperty<string> InAppPrice = new ();
    public readonly ReactiveProperty<string> InAppIcon = new ();
    
    private readonly IPickupCollector _collector;
    private readonly ChestConfig _chestConfig;
    private readonly MainGUIModel _mainGUI;

    public ChestInfoModel(IPickupCollector collector, ChestConfig chestConfig, MainGUIModel mainGUI)
    {
      _collector = collector;
      _chestConfig = chestConfig;
      _mainGUI = mainGUI;
      ChestName.Value = new TextData(chestConfig.ChestName);
      ChestIcon.Value = chestConfig.ChestIcon;
      BackgroundColor.Value = chestConfig.Color;
      PickaxeVariants.AddRange(chestConfig.Variants);
      Cost.Value = (long)ServiceProvider.Get<ChestService>().GetChestCost(chestConfig.Type);
      ShowInApp.Value = !ServiceProvider.Get<ChestService>().CanOpen(chestConfig);

      var price = ServiceProvider.Get<InAppService>().GetPrice(chestConfig.InApp, out var icon);
      InAppPrice.Value = price;
      InAppIcon.Value = icon;
    }

    public void Open()
    {
      ServiceProvider.Get<ChestService>().TryOpenChest(_chestConfig);
    }

    public void Discard()
    {
      _collector.Discard();
      _mainGUI.HideChestInfo();
    }
  }
}