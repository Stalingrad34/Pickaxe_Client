using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Gameplay.ECS.Pickup.Interfaces;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Widgets;
using UniRx;

namespace Game.Scripts.UI.GUI
{
  public class ChestInfoModel : WidgetModel
  {
    public readonly ReactiveProperty<TextData> ChestName = new ();
    private readonly IPickupCollector _collector;
    private readonly MainGUIModel _mainGUI;

    public ChestInfoModel(IPickupCollector collector, ChestConfig chestConfig, MainGUIModel mainGUI)
    {
      _collector = collector;
      _mainGUI = mainGUI;
      ChestName.Value = new TextData(chestConfig.ChestName);
    }

    public void Discard()
    {
      _collector.Discard();
      _mainGUI.HideChestInfo();
    }
  }
}