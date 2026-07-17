using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Widgets;
using UniRx;
using UnityEngine;

namespace Game.Scripts.UI.Popups.Collection
{
  public class CollectionItemModel : WidgetModel
  {
    public readonly ReactiveProperty<TextData> Name = new ();
    public readonly ReactiveProperty<Color> NameColor = new ();
    public readonly ReactiveProperty<Sprite> Icon = new ();

    public CollectionItemModel(PickaxeConfig pickaxe, bool isCollected)
    {
      Name.Value = isCollected ? pickaxe.nameKey : string.Empty;
      NameColor.Value = pickaxe.oreConfig?.pickupColor ?? Color.white;
      Icon.Value = isCollected ? pickaxe.available : pickaxe.unavailable;
    }
  }
}