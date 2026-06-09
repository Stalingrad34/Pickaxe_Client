using BitGames.Bits;
using Game.Scripts.Infrastructure.Services.Database;
using UniRx;

namespace Game.Scripts.UI.GUI
{
  public class MainGUIModel : GUIModel
  {
    public readonly ReactiveProperty<ulong> Money;
    public readonly ReactiveProperty<bool> ShowJoystick = new ();
    private readonly DatabaseService _database;

    public MainGUIModel(DatabaseService database)
    {
      _database = database;
      Money = _database.Money;
      ShowJoystick.Value = Platform.IsMobileWebGL();
    }
  }
}