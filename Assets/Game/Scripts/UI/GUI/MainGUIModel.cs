using BitGames.Bits;
using Game.Scripts.Infrastructure.Services;
using UniRx;

namespace Game.Scripts.UI.GUI
{
  public class MainGUIModel : GUIModel
  {
    public readonly ReactiveProperty<ulong> Money;
    public readonly ReactiveProperty<ulong> Ore;
    public readonly ReactiveProperty<bool> ShowJoystick = new ();
    public readonly ReactiveCommand<PickupTextData> PickupTextCommand;
    private readonly EconomyService _economy;

    public MainGUIModel(EconomyService economy)
    {
      _economy = economy;
      Money = _economy.Money;
      Ore = _economy.Ore;
      ShowJoystick.Value = Platform.IsMobileWebGL();
      PickupTextCommand = economy.PickupTextCommand;
    }
  }
}