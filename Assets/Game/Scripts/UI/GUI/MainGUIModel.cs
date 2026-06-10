using BitGames.Bits;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.Database;
using UniRx;

namespace Game.Scripts.UI.GUI
{
  public class MainGUIModel : GUIModel
  {
    public readonly ReactiveProperty<ulong> Money;
    public readonly ReactiveProperty<bool> ShowJoystick = new ();
    private readonly EconomyService _economy;

    public MainGUIModel(EconomyService economy)
    {
      _economy = economy;
      Money = _economy.Money;
      ShowJoystick.Value = Platform.IsMobileWebGL();
    }
  }
}