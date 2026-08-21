using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.InApp;
using Game.Scripts.Infrastructure.Widgets;
using UniRx;

namespace Game.Scripts.UI.GUI
{
  public class MoneyInAppModel : WidgetModel
  {
    public readonly ReactiveProperty<ulong> Money = new();
    public readonly ReactiveProperty<string> InAppPrice = new ();
    public readonly ReactiveProperty<string> InAppIcon = new ();
    private readonly string _inApp;
    private readonly AddMoneyGrade _grade;

    public MoneyInAppModel(string inApp, AddMoneyGrade grade)
    {
      _inApp = inApp;
      _grade = grade;
      ServiceProvider.Get<PickaxesService>().BestPickaxeType.Subscribe(BestPickaxeTypeChanged).AddTo(disposables);
      
      var price = ServiceProvider.Get<InAppService>().GetPrice(_inApp, out var icon);
      InAppPrice.Value = price;
      InAppIcon.Value = icon;
    }

    private void BestPickaxeTypeChanged(PickaxeType pickaxeType)
    {
      Money.Value = ServiceProvider.Get<InAppService>().GetMoneyCountByGrade(_grade);
    }

    public void Buy()
    {
      ServiceProvider.Get<InAppService>().BuyInApp(_inApp);
    }
  }
}