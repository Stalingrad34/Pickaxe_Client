using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Gameplay.ECS.Pickup.Interfaces;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Infrastructure;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.InApp;
using Game.Scripts.Infrastructure.UI;
using Game.Scripts.UI.Popups.Collection;
using UniRx;

namespace Game.Scripts.UI.GUI
{
  public class MainGUIModel : GUIModel
  {
    public readonly ReactiveProperty<ulong> Money;
    public readonly ReactiveProperty<ulong> Ore;
    public readonly ReactiveProperty<ulong> Pickaxes;
    public readonly ReactiveProperty<bool> ShowJoystick = new ();
    public readonly ReactiveProperty<int> CollectedPickaxesMaxCount = new ();
    public readonly ReactiveProperty<int> CollectedPickaxesCurrentCount = new ();
    public readonly ReactiveProperty<ChestInfoModel> ChestInfo = new ();
    public readonly ReactiveCommand<PickupTextData> PickupTextCommand;
    public readonly ReactiveCommand<PickupTextData> MoneyTextCommand;
    public readonly ReactiveCommand<PickaxeConfig> ShowOpenVFX = new ();
    public readonly ReactiveCommand<PickaxeConfig> NewPickaxeVFX = new ();
    public readonly ReactiveCollection<MoneyInAppModel> MoneyInApps = new();

    public MainGUIModel(EconomyService economy, PickaxesService pickaxesService)
    {
      Money = economy.Money;
      Ore = economy.Ore;
      Pickaxes = pickaxesService.PickaxesNominal;
      ShowJoystick.Value = Platform.IsMobileWebGL();
      PickupTextCommand = economy.PickupTextCommand;
      MoneyTextCommand = economy.MoneyTextCommand;
      CollectedPickaxesMaxCount.Value = AssetProvider.GetAllPickaxes().Count;
      ServiceProvider.Get<PickaxesService>().CollectedPickaxes.SubscribeCount(CollectedPickaxesCountChanged).AddTo(disposables);
      ServiceProvider.Get<PickaxesService>().CollectedPickaxes.SubscribeAdd(CollectedPickaxesAdded).AddTo(disposables);
      
      MoneyInApps.Add(new MoneyInAppModel("add_money_1", AddMoneyGrade.Grade1));
      MoneyInApps.Add(new MoneyInAppModel("add_money_2", AddMoneyGrade.Grade2));
      MoneyInApps.Add(new MoneyInAppModel("add_money_3", AddMoneyGrade.Grade3));
    }

    public void OpenCollection()
    {
      var model = new CollectionPopupModel();
      UIManager.ShowPopup<CollectionPopupView, CollectionPopupModel>(model);
    }

    private void CollectedPickaxesCountChanged(int count)
    {
      CollectedPickaxesCurrentCount.Value = count;
    }

    private void CollectedPickaxesAdded(PickaxeType pickaxeType, int index)
    {
      var config = AssetProvider.GetPickaxeData(pickaxeType);
      NewPickaxeVFX.Execute(config);
    }

    public void ShowChestInfo(IPickupCollector collector, ChestConfig chestConfig)
    {
      ChestInfo.Value = new ChestInfoModel(collector, chestConfig, this);
    }

    public void HideChestInfo()
    {
      ChestInfo.Value = null;
    }
  }
}