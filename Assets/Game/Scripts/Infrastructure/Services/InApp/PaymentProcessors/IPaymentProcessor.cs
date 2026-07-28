namespace Game.Scripts.Infrastructure.Services.InApp.PaymentProcessors
{
  public interface IPaymentProcessor
  {
    bool TryProcess(string inApp);
  }
}