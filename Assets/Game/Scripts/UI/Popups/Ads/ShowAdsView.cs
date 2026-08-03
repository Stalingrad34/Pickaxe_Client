using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.UI;
using TMPro;
using UniRx;
using UnityEngine;
using YG;

namespace Game.Scripts.UI.Popups.Ads
{
  public class ShowAdsView : PopupView<ShowAdsModel>
  {
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject countdownRoot;
    [SerializeField] private GameObject continueRoot;
    [SerializeField] private GameObject noAdsRoot;
    [SerializeField] private ImageLoadYG noAdsIcon;
    [SerializeField] private TextMeshProUGUI noAdsPrice;
    [SerializeField] private CustomButton continueBtn;
    [SerializeField] private CustomButton buyNoAdsBtn;
    
    protected override void SetModel(ShowAdsModel model)
    {
      model.Countdown.SubscribeToTMP(countdownText).AddTo(gameObject);
      model.NoAdsPrice.SubscribeToTMP(noAdsPrice).AddTo(gameObject);
      model.NoAdsIcon.Subscribe(NoAdsIconChanged).AddTo(gameObject);
      model.ShowNoAds.Subscribe(NoAdsChanged).AddTo(gameObject);
      model.AdsShowed.Subscribe(AdsShowedChanged).AddTo(gameObject);
      
      continueBtn.OnClick(model.Continue).AddTo(gameObject);
      buyNoAdsBtn.OnClick(model.BuyNoAds).AddTo(gameObject);
    }

    private void AdsShowedChanged(bool isShowed)
    {
      countdownRoot.SetActive(!isShowed);
      continueRoot.SetActive(isShowed);
    }

    private void NoAdsChanged(bool noAds)
    {
      noAdsRoot.SetActive(!noAds);
    }
    
    private void NoAdsIconChanged(string icon)
    {
      noAdsIcon.Load(icon);
    }
  }
}