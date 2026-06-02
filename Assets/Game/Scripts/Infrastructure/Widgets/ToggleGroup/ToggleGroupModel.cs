using System;
using UniRx;

namespace Game.Scripts.Widgets.ToggleGroup
{
  public class ToggleGroupModel<T> where T : Enum
  {
    public readonly ReactiveProperty<T> CurrentPage = new();
    private readonly IToggleReceiver<T> _toggleReceiver;
    private T _currentPage;
    
    public ToggleGroupModel(T currentPage, IToggleReceiver<T> toggleReceiver)
    {
      CurrentPage.Value = currentPage;
      _toggleReceiver = toggleReceiver;
      _toggleReceiver.ReceiveToggle(currentPage);
    }

    public void OnClicked(T toggleType)
    {
      CurrentPage.Value = toggleType;
      _toggleReceiver.ReceiveToggle(toggleType);
    }
  }
}