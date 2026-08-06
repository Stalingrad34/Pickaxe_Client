using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Gameplay.VFX;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.UI;
using TMPro;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.UI.GUI
{
  public class MainGUIView : GUIView<MainGUIModel>
  {
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI oreText;
    [SerializeField] private CustomText pickaxesCountText;
    [SerializeField] private GameObject joystick;
    [SerializeField] private RectTransform pickupTextArea;
    [SerializeField] private RectTransform pickupTextTarget;
    [SerializeField] private TextMeshProUGUI pickupTextView;
    [SerializeField] private AnimationCurve pickupTextAlphaCurve;
    [SerializeField] private CustomButton collectionBtn;
    [SerializeField] private TextMeshProUGUI collectedPickaxesCount;
    [SerializeField] private ChestInfoWidget chestInfoWidget;
    [SerializeField] private OpenChestVFX openVFX;
    [SerializeField] private NewPickaxeVFX newPickaxeVFX;
    [SerializeField] private List<MoneyInAppView> moneyInApps;
    [SerializeField] private CustomText tutorialText;
    [SerializeField] private float pickupTextDuration;
    [SerializeField] private TextAnimationView pickupTextAnimation;
    [SerializeField] private TextAnimationView decreaseTextAnimation;

    private int _collectedMaxCount;
    private int _collectedCurrentCount;

    protected override void SetModel(MainGUIModel model)
    {
      model.Money.SubscribeMoney(moneyText).AddTo(gameObject);
      model.Ore.SubscribeOre(oreText).AddTo(gameObject);
      model.Pickaxes.Subscribe(PickaxesCountChanged).AddTo(gameObject);
      model.ShowJoystick.Subscribe(joystick.SetActive).AddTo(gameObject);
      model.PickupTextCommand.Subscribe(pickupTextAnimation.PickupTextHandler).AddTo(gameObject);
      model.MoneyTextCommand.Subscribe(decreaseTextAnimation.PickupTextHandler).AddTo(gameObject);
      model.CollectedPickaxesMaxCount.Subscribe(CollectedPickaxesMaxChanged).AddTo(gameObject);
      model.CollectedPickaxesCurrentCount.Subscribe(CollectedPickaxesCurrentChanged).AddTo(gameObject);
      model.ChestInfo.Subscribe(ChestInfoChanged).AddTo(gameObject);
      model.ShowOpenVFX.Subscribe(ShowOpenVFX).AddTo(gameObject);
      model.NewPickaxeVFX.Subscribe(ShowNewPickaxeVFX).AddTo(gameObject);
      model.MoneyInApps.SubscribeAdd(MoneyInAppsChanged).AddTo(gameObject);

      collectionBtn.OnClick(model.OpenCollection).AddTo(gameObject);
    }

    private void CollectedPickaxesMaxChanged(int count)
    {
      _collectedMaxCount = count;
      collectedPickaxesCount.text = $"{_collectedCurrentCount} / {_collectedMaxCount}";
    }
    
    private void CollectedPickaxesCurrentChanged(int count)
    {
      _collectedCurrentCount = count;
      collectedPickaxesCount.text = $"{_collectedCurrentCount} / {_collectedMaxCount}";
    }

    private void PickaxesCountChanged(ulong count)
    {
      pickaxesCountText.SetText(new TextData("pickaxes_count", count.ToString()));
    }

    

    private void ChestInfoChanged(ChestInfoModel chestInfoModel)
    {
      if (chestInfoModel == null)
      {
        chestInfoWidget.gameObject.SetActive(false);
        chestInfoWidget.Clear();
        return;
      }
      
      chestInfoWidget.gameObject.SetActive(true);
      chestInfoWidget.Init(chestInfoModel);
    }

    private void MoneyInAppsChanged(MoneyInAppModel model, int index)
    {
      if (index > moneyInApps.Count - 1)
        return;
      
      moneyInApps[index].Init(model);
    }
    
    private void ShowOpenVFX(PickaxeConfig pickaxeConfig)
    {
      openVFX.ShowPickaxe(pickaxeConfig);
    }
    
    private void ShowNewPickaxeVFX(PickaxeConfig pickaxeConfig)
    {
      newPickaxeVFX.ShowVFX(pickaxeConfig);
    }

    public void ShowTutorialText(TextData text)
    {
      tutorialText.SetText(text);
    }

    public void HideTutorialText()
    {
      tutorialText.SetText(string.Empty);
    }
  }
}