using System;

namespace Game.Scripts.Widgets.ToggleGroup
{
  public interface IToggleReceiver<in T> where T : Enum
  {
    void ReceiveToggle(T value);
  }
}