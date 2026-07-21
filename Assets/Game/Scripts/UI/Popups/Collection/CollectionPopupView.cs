using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.UI;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.UI.Popups.Collection
{
  public class CollectionPopupView : PopupView<CollectionPopupModel>
  {
    [SerializeField] private TextMeshProUGUI collectedCount;
    [SerializeField] private CollectionItemView itemPrefab;
    [SerializeField] private RectTransform itemsRoot;
    [SerializeField] private CustomButton closeBtn;
    
    private int _collectedMaxCount;
    private int _collectedCurrentCount;
    
    protected override void SetModel(CollectionPopupModel model)
    {
      model.Items.SubscribeAdd(ItemsChanged).AddTo(gameObject);
      model.CollectedPickaxesMaxCount.Subscribe(CollectedPickaxesMaxChanged).AddTo(gameObject);
      model.CollectedPickaxesCurrentCount.Subscribe(CollectedPickaxesCurrentChanged).AddTo(gameObject);
      
      closeBtn.OnClick(model.Close).AddTo(gameObject);
    }

    private void ItemsChanged(CollectionItemModel item, int index)
    {
      var view = Instantiate(itemPrefab, itemsRoot);
      view.gameObject.SetActive(true);
      view.Init(item);
    }
    
    private void CollectedPickaxesMaxChanged(int count)
    {
      _collectedMaxCount = count;
      collectedCount.text = $"{_collectedCurrentCount} / {_collectedMaxCount}";
    }
    
    private void CollectedPickaxesCurrentChanged(int count)
    {
      _collectedCurrentCount = count;
      collectedCount.text = $"{_collectedCurrentCount} / {_collectedMaxCount}";
    }
  }
}