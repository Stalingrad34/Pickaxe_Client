using System;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Infrastructure.Services.InApp;
using Game.Scripts.Infrastructure.UI;
using Game.Scripts.UI.GUI;

namespace Game.Scripts.Infrastructure.Services
{
  public class ChestService : IService
  {
    public bool CanOpen(ChestConfig chestConfig)
    {
      var cost = GetChestCost(chestConfig.Type);
      return ServiceProvider.Get<EconomyService>().Money.Value >= cost;
    }
    
    public ulong GetChestCost(ChestType chestType)
    {
      var pickaxes = ServiceProvider.Get<PickaxesService>().PickaxesNominal.Value;
      ulong cost = GetWoodChestCost(pickaxes);
      int tier = GetChestTier(chestType);

      for (int i = 0; i < tier; i++)
        cost = MultiplyBy301(cost);

      return cost;
    }

    private ulong GetWoodChestCost(ulong pickaxes)
    {
      double cost = 72d * Math.Pow(pickaxes + 30d, 0.85d) - 1000d;

      if (cost <= 0d)
        return 0;

      if (cost >= ulong.MaxValue)
        return ulong.MaxValue;

      return (ulong)cost;
    }

    private int GetChestTier(ChestType chestType)
    {
      return chestType switch
      {
        ChestType.Wood => 0,
        ChestType.Shiny => 1,
        ChestType.Rare => 2,
        _ => 0
      };
    }

    private ulong MultiplyBy301(ulong value)
    {
      if (value > ulong.MaxValue / 301UL)
        return ulong.MaxValue;

      return value * 301UL / 10UL;
    }
    
    public void TryOpenChest(ChestConfig chestConfig)
    {
      if (CanOpen(chestConfig))
      {
        var cost = GetChestCost(chestConfig.Type);
        ServiceProvider.Get<EconomyService>().DecreaseMoney(cost);
        OpenChest(chestConfig);
      }
      else if (chestConfig.CanAds)
        ServiceProvider.Get<AdsService>().ShowRewarded("open_chest", () => OpenChest(chestConfig));
      else
        ServiceProvider.Get<InAppService>().BuyInApp(chestConfig.InApp);
    }
    
    public void OpenChest(ChestConfig chestConfig, bool byInApp = false)
    {
      var pickaxeConfig = GetRandomPickaxeConfig(chestConfig);
      ServiceProvider.Get<PickaxesService>().AddPickaxe(pickaxeConfig.pickaxeType, 1);

      var guiModel = UIManager.GetGUI<MainGUIModel>();
      guiModel?.ShowOpenVFX.Execute(pickaxeConfig);
      guiModel?.ChestInfo?.Value.Discard();

      var chestType = chestConfig.Type.ToString();
      if (byInApp)
        ServiceProvider.Get<AnalyticsService>().MetricaSend("inapp", "Chest", chestType);
      else
        ServiceProvider.Get<AnalyticsService>().MetricaSend("chest", "Open", chestType);
    }
    
    private PickaxeConfig GetRandomPickaxeConfig(ChestConfig chestConfig)
    {
      var items = new List<WeightedItem<PickaxeConfig>>();
      foreach (var variant in chestConfig.Variants)
      {
        items.Add(new WeightedItem<PickaxeConfig>(variant.pickaxeConfig, variant.Chance));
      }

      return items.GetWeightedRandom();
    }
  }
}