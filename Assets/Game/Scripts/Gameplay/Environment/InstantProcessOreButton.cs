using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.Config;
using UniRx;

namespace Game.Scripts.Gameplay.Environment
{
  public class InstantProcessOreButton : WorldInteractableButton
  {
    private bool _isEnabled;

    public override void Awake()
    {
      base.Awake();
      _isEnabled = ServiceProvider.Get<ConfigProvider>().EnableRewarded;
      if (_isEnabled)
        ServiceProvider.Get<EconomyService>().ProcessingOre.Subscribe(ProcessingOreChanged).AddTo(gameObject);
      else
        gameObject.SetActive(false);
    }

    protected override void OnClick()
    {
      ServiceProvider.Get<OreProcessingService>().InstantProcessOreByAds();
    }

    private void ProcessingOreChanged(ulong amount)
    {
      gameObject.SetActive(amount > 0);
    }
  }
}