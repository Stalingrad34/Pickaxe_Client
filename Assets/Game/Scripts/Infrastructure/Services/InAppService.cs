using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;

namespace Game.Scripts.Infrastructure.Services
{
  public class InAppService : IInitializableService
  {
    public async UniTask Init(CancellationToken token)
    {
      YG2.onPurchaseSuccess += PurchaseSuccess;
      YG2.ConsumePurchases();
      
      await UniTask.Yield(token).SuppressCancellationThrow();
    }
    
    public void BuyInApp(string inApp)
    {
      YG2.BuyPayments(inApp);
    }

    private void PurchaseSuccess(string id)
    {
      /*var allWeapons = AssetProvider.GetAllWeapons();
      var weapon = allWeapons.FirstOrDefault(w => w.InApp == id);
      if (weapon)
      {
        ServiceProvider.Get<DatabaseService>().Inventory.Add(weapon.Id);
        var data = new Dictionary<string, object>();
        data["ByInApp"] = weapon.Id;
        ServiceProvider.Get<AnalyticsService>().MetricaSend("buy_weapon", data);
      }*/
    }

    public string GetCost(string inApp, out string priceIcon)
    {
      priceIcon = string.Empty;
      var purchase = YG2.purchases.FirstOrDefault(p => p.id ==  inApp);
      if (purchase == null)
        return string.Empty;
      
      priceIcon = purchase.currencyImageURL;
      return purchase.priceValue;
    }
  }
}
