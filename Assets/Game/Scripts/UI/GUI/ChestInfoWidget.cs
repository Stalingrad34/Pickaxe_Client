using System.Collections.Generic;
using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Widgets;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.GUI
{
  public class ChestInfoWidget : WidgetView<ChestInfoModel>
  {
    [SerializeField] private CustomText chestName;
    [SerializeField] private CustomText cost;
    [SerializeField] private Image chestIcon;
    [SerializeField] private Image chestBackground;
    [SerializeField] private PickaxeVariantView pickaxeVariant;
    [SerializeField] private RectTransform pickaxeVariantsRoot;
    [SerializeField] private CustomButton open;
    [SerializeField] private CustomButton discard;

    private readonly List<PickaxeVariantView> _variantViews = new();
    
    protected override void SetModel(ChestInfoModel model)
    {
      model.ChestName.SubscribeCustomText(chestName).AddTo(gameObject);
      model.ChestIcon.SubscribeToImageSprite(chestIcon).AddTo(gameObject);
      model.BackgroundColor.SubscribeColor(chestBackground).AddTo(gameObject);
      model.PickaxeVariants.SubscribeAdd(PickaxeVariantAdded).AddTo(gameObject);

      open.OnClick(model.Open).AddTo(Disposables);
      discard.OnClick(model.Discard).AddTo(Disposables);
    }

    public void Clear()
    {
      _variantViews.ForEach(v => Destroy(v.gameObject));
      _variantViews.Clear();
    }

    private void PickaxeVariantAdded(PickaxeVariant variant, int index)
    {
      var view = Instantiate(pickaxeVariant, pickaxeVariantsRoot);
      view.gameObject.SetActive(true);
      view.Init(variant);
      _variantViews.Add(view);
    }
  }
}