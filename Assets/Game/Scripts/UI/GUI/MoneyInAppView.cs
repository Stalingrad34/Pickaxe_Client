using Game.Scripts.Gameplay;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Widgets;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.UI.GUI
{
  public class MoneyInAppView : WidgetView<MoneyInAppModel>
  {
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private CustomButton actionBtn;
    
    protected override void SetModel(MoneyInAppModel model)
    {
      model.Money.Subscribe(MoneyChanged).AddTo(gameObject);
      actionBtn.OnClick(model.Buy).AddTo(gameObject);
    }

    private void MoneyChanged(ulong money)
    {
      moneyText.text = $"+{MoneyFormatter.Format((long)money)}";
    }
  }
}