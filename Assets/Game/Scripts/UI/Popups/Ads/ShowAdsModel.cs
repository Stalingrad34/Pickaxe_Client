using System;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.InApp;
using Game.Scripts.Infrastructure.UI;
using UniRx;
using UnityEngine;

namespace Game.Scripts.UI.Popups.Ads
{
  public class ShowAdsModel : PopupModel
  {
    public readonly ReactiveProperty<int> Countdown = new();
    public readonly ReactiveProperty<bool> AdsShowed = new();
    public readonly ReactiveProperty<bool> ShowNoAds;
    public readonly ReactiveProperty<string> NoAdsPrice = new();
    public readonly ReactiveProperty<string> NoAdsIcon = new();
    private IDisposable _timer;

    public ShowAdsModel(int countdown)
    {
      Countdown.Value = countdown;
      ShowNoAds = ServiceProvider.Get<AdsService>().NoAds;
      NoAdsPrice.Value = ServiceProvider.Get<InAppService>().GetPrice("no_ads", out var icon);
      NoAdsIcon.Value = icon;
      Debug.Log($"Price icon {icon}");
    }

    public void StartCountdown()
    {
      _timer = Observable
        .Timer(TimeSpan.FromSeconds(1))
        .Repeat()
        .Subscribe(_ =>
        {
          Countdown.Value--;

          if (Countdown.Value <= 0)
            ShowAds();
        });
    }

    public void Continue()
    {
      ServiceProvider.Get<AdsService>().StartAdsTimer();
      Close();
    }

    public void BuyNoAds()
    {
      ServiceProvider.Get<InAppService>().BuyInApp("no_ads");
    }

    private void ShowAds()
    {
      ServiceProvider.Get<AdsService>().ShowInterstitial();
      AdsShowed.Value = true;
      _timer?.Dispose();
    }
  }
}