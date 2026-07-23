using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Gameplay.ECS.Pickup.Interfaces;
using Game.Scripts.Infrastructure;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Services;
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

    public MainGUIModel(EconomyService economy, PickaxesService pickaxesService)
    {
      Money = economy.Money;
      Ore = economy.Ore;
      Pickaxes = pickaxesService.PickaxesNominal;
      ShowJoystick.Value = Platform.IsMobileWebGL();
      PickupTextCommand = economy.PickupTextCommand;
      CollectedPickaxesMaxCount.Value = AssetProvider.GetAllPickaxes().Count;
      ServiceProvider.Get<PickaxesService>().CollectedPickaxes.SubscribeCount(CollectedPickaxesCountChanged).AddTo(disposables);
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