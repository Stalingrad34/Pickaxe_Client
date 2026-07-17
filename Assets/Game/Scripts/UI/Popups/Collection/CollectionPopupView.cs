using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Extensions;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.UI.Popups.Collection
{
  public class CollectionPopupView : PopupView<CollectionPopupModel>
  {
    [SerializeField] private TextMeshProUGUI availableCount;
    [SerializeField] private CollectionItemView itemPrefab;
    [SerializeField] private RectTransform itemsRoot;
    [SerializeField] private CustomButton closeBtn;
    
    protected override void SetModel(CollectionPopupModel model)
    {
      model.Items.SubscribeAdd(ItemsChanged).AddTo(gameObject);
      closeBtn.OnClick(model.Close).AddTo(gameObject);
    }

    private void ItemsChanged(CollectionItemModel item, int index)
    {
      var view = Instantiate(itemPrefab, itemsRoot);
      view.gameObject.SetActive(true);
      view.Init(item);
    }
  }
}