using System;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using Game.Scripts.Infrastructure.UI;
using Game.Scripts.UI.Popups.Ads;
using UniRx;
using YG;

namespace Game.Scripts.Infrastructure.Services
{
  public class AdsService : IService, IStorageProcessor
  {
    public bool IsDirty { get; private set; }
    
    public readonly ReactiveProperty<bool> NoAds = new();
    private IDisposable _timer;

    public void StartAdsTimer()
    {
      if (NoAds.Value)
        return;
      
      _timer?.Dispose();
      _timer = Observable
        .Timer(TimeSpan.FromMinutes(1))
        .Subscribe(_ => ShowAdsPopup());
    }
    
    public void ShowInterstitial()
    {
      YG2.InterstitialAdvShow();
    }

    public void ShowRewarded(string id, Action callback)
    {
      YG2.RewardedAdvShow(id, callback);
    }

    private void ShowAdsPopup()
    {
      var model = new ShowAdsModel(3);
      UIManager.ShowPopup<ShowAdsView, ShowAdsModel>(model);
      
      model.StartCountdown();
    }
    
    public void Save(SaveData data)
    {
      data.Ads.NoAds = NoAds.Value;
      IsDirty = false;
    }

    public void Load(SaveData data)
    {
      NoAds.Value = data.Ads.NoAds;
      Subscribe();
    }

    private void Subscribe()
    {
      NoAds.Subscribe(_ => IsDirty = true);
    }
  }
}