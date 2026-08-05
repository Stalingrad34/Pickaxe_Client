using Game.Scripts.Gameplay.Chest;

namespace Game.Scripts.Infrastructure.Services.InApp.PaymentProcessors
{
  public class OpenChestPaymentProcessor : IPaymentProcessor
  {
    private readonly string _inApp;
    private readonly ChestConfig _chestConfig;

    public OpenChestPaymentProcessor(string inApp, ChestConfig chestConfig)
    {
      _inApp = inApp;
      _chestConfig = chestConfig;
    }

    public bool TryProcess(string inApp)
    {
      if (_inApp != inApp)
        return false;
      
      ServiceProvider.Get<ChestService>().OpenChest(_chestConfig, true);

      return true;
    }
  }
}