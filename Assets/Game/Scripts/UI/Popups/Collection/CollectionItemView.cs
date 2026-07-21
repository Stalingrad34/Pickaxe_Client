using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Widgets;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Popups.Collection
{
  public class CollectionItemView : WidgetView<CollectionItemModel>
  {
    [SerializeField] private CustomText nameText;
    [SerializeField] private Image icon;
    
    protected override void SetModel(CollectionItemModel model)
    {
      model.Name.SubscribeCustomText(nameText).AddTo(gameObject);
      model.NameColor.SubscribeTextColor(nameText).AddTo(gameObject);
      model.Icon.SubscribeToImageSprite(icon).AddTo(gameObject);
    }
  }
}