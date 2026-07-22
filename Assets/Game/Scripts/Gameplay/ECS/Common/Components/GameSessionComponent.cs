using Game.Scripts.States;
using Game.Scripts.UI.GUI;

namespace Game.Scripts.Gameplay.ECS.Common.Components
{
  public struct GameSessionComponent
  {
    public GameState GameState;
    public MainGUIModel MainGUIModel;
  }
}