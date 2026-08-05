namespace Game.Scripts.Infrastructure.Services.InApp.PaymentProcessors
{
  public class AddMoneyPaymentProcessor : IPaymentProcessor
  {
    private readonly string _inApp;
    private readonly AddMoneyGrade _grade;

    public AddMoneyPaymentProcessor(string inApp, AddMoneyGrade grade)
    {
      _inApp = inApp;
      _grade = grade;
    }
    
    public bool TryProcess(string inApp)
    {
      if (inApp != _inApp)
        return false;
      
      var money = ServiceProvider.Get<InAppService>().GetMoneyCountByGrade(_grade);
      ServiceProvider.Get<EconomyService>().Money.Value += money;
      ServiceProvider.Get<AnalyticsService>().MetricaSend("inapp", "Money", _grade.ToString());
      
      return true;
    }
  }
}