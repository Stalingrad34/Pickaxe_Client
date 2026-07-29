using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services.InApp.PaymentProcessors;
using YG;

namespace Game.Scripts.Infrastructure.Services.InApp
{
  public class InAppService : IInitializableService
  {
    private const ulong BASE_MONEY_MULTIPLIER = 75; 
    private List<IPaymentProcessor> _paymentProcessors;
    
    public async UniTask Init(CancellationToken token)
    {
      YG2.onPurchaseSuccess += PurchaseSuccess;
      YG2.ConsumePurchases();
      
      _paymentProcessors = GetPaymentProcessors();
      
      await UniTask.Yield(token).SuppressCancellationThrow();
    }

    public void BuyInApp(string inApp)
    {
      YG2.BuyPayments(inApp);
    }

    private void PurchaseSuccess(string id)
    {
      foreach (var processor in _paymentProcessors)
      {
        if (processor.TryProcess(id))
          return;
      }
    }

    public string GetPrice(string inApp, out string priceIcon)
    {
      priceIcon = string.Empty;
      var purchase = YG2.purchases.FirstOrDefault(p => p.id ==  inApp);
      if (purchase == null)
        return string.Empty;
      
      priceIcon = purchase.currencyImageURL;
      return purchase.priceValue;
    }

    public ulong GetMoneyCountByGrade(AddMoneyGrade grade)
    {
      var bestPickaxe = ServiceProvider.Get<PickaxesService>().BestPickaxeType.Value;
      var result = BASE_MONEY_MULTIPLIER;
     
      for (int i = 1; i < (int)bestPickaxe; i++)
        result *= 5;

      for (int i = 1; i < (int)grade; i++)
        result *= 50;
      
      return result;
    }
    
    private List<IPaymentProcessor> GetPaymentProcessors()
    {
      return new List<IPaymentProcessor>()
      {
        new OpenChestPaymentProcessor("open_chest_wood", AssetProvider.GetChest("Chest_wood")),
        new OpenChestPaymentProcessor("open_chest_shiny", AssetProvider.GetChest("Chest_shiny")),
        new OpenChestPaymentProcessor("open_chest_rare", AssetProvider.GetChest("Chest_rare")),
        new AddMoneyPaymentProcessor("add_money_1", AddMoneyGrade.Grade1),
        new AddMoneyPaymentProcessor("add_money_2", AddMoneyGrade.Grade2),
        new AddMoneyPaymentProcessor("add_money_3", AddMoneyGrade.Grade3),
      };
    }
  }
}
