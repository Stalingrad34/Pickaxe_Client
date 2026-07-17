using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Widgets;
using UniRx;
using UnityEngine;

namespace Game.Scripts.UI.Popups.Collection
{
  public class CollectionItemModel : WidgetModel
  {
    public readonly ReactiveProperty<TextData> Name = new ();
    public readonly ReactiveProperty<Sprite> Icon = new ();

    public CollectionItemModel(string name, Sprite icon)
    {
      Name.Value = name;
      Icon.Value = icon;
    }
  }
}