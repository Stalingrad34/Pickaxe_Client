namespace Game.Scripts.Infrastructure.Services.InApp.PaymentProcessors
{
  public class NoAdsPaymentProcessor : IPaymentProcessor
  {
    private readonly string _inApp;

    public NoAdsPaymentProcessor(string inApp)
    {
      _inApp = inApp;
    }
    
    public bool TryProcess(string inApp)
    {
      if (_inApp != inApp)
        return false;

      ServiceProvider.Get<AdsService>().SetNoAds(true);
      ServiceProvider.Get<AnalyticsService>().MetricaSend("inapp", "Ads","NoAds");
      return true;
    }
  }
}