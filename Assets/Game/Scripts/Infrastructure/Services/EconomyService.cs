using Game.Scripts.Gameplay;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using Game.Scripts.Infrastructure.Sound;
using Game.Scripts.UI.GUI;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services
{
  public class EconomyService : IService, IStorageProcessor
  {
    public bool IsDirty { get; private set; }
    
    public readonly ReactiveProperty<ulong> Money = new();
    public readonly ReactiveProperty<ulong> Ore = new();
    public readonly ReactiveProperty<ulong> ProcessingMoney = new();
    public readonly ReactiveProperty<ulong> ProcessingOre = new();
    public readonly ReactiveProperty<ulong> RestOre = new();
    public readonly ReactiveProperty<int> ProcessingStage = new();
    public readonly ReactiveCommand<PickupTextData> PickupTextCommand = new();
    public readonly ReactiveCommand<PickupTextData> MoneyTextCommand = new();

    public void AddOre(ulong amount, Color pickupTextColor)
    {
      Ore.Value += amount;
      var pickupData = new PickupTextData()
      {
        Text = $"+{MoneyFormatter.Format((long)amount)}",
        Color = pickupTextColor
      };
      PickupTextCommand.Execute(pickupData);
      AudioController.Instance.PlayAudioClipFromSoundMap("pickup_ore");
    }

    public void AddRestOre(long amount)
    {
      RestOre.Value += (ulong)amount;
    }
    
    public void IncreaseMoney(ulong money)
    {
      Money.Value += money;
      var pickupData = new PickupTextData()
      {
        Text = $"+${MoneyFormatter.Format((long)money)}",
        Color = Color.green
      };
      MoneyTextCommand.Execute(pickupData);
      AudioController.Instance.PlayAudioClipFromSoundMap("collect");
    }
    
    public void DecreaseMoney(ulong cost)
    {
      Money.Value -= cost;
      var pickupData = new PickupTextData()
      {
        Text = $"-${MoneyFormatter.Format((long)cost)}",
        Color = Color.red
      };
      MoneyTextCommand.Execute(pickupData);
    }

    public void ConvertRestOre()
    {
      var restOreText = MoneyFormatter.Format((long)RestOre.Value);
      ServiceProvider.Get<AnalyticsService>().MetricaSend("offline", "OreCount", restOreText);
      AddOre(RestOre.Value, Color.greenYellow);
      RestOre.Value = 0;
    }
    
    public void CollectMoney()
    {
      if (ProcessingMoney.Value == 0)
        return;
      
      var moneyText = MoneyFormatter.Format((long)ProcessingMoney.Value);
      ServiceProvider.Get<AnalyticsService>().MetricaSend("collect", "Money", moneyText);
      IncreaseMoney(ProcessingMoney.Value);
      ProcessingMoney.Value = 0;
    }
    
    public void IncreaseStage(ulong cost)
    {
      if (cost > Money.Value)
        return;
      
      DecreaseMoney(cost);
      ProcessingStage.Value++;
      ServiceProvider.Get<AnalyticsService>().MetricaSend("stage", "NewStage", ProcessingStage.Value.ToString());
      AudioController.Instance.PlayAudioClipFromSoundMap("stage");
    }

    public void Save(SaveData data)
    {
      data.Economy.Money = Money.Value;
      data.Economy.Ore = Ore.Value;
      data.Economy.RestOre = RestOre.Value;
      data.Economy.ProcessingMoney = ProcessingMoney.Value;
      data.Economy.ProcessingOre = ProcessingOre.Value;
      data.Economy.ProcessingStage = ProcessingStage.Value;

      IsDirty = false;
    }

    public void Load(SaveData data)
    {
      Money.Value = data.Economy.Money;
      Ore.Value = data.Economy.Ore;
      RestOre.Value = data.Economy.RestOre;
      ProcessingMoney.Value = data.Economy.ProcessingMoney;
      ProcessingOre.Value = data.Economy.ProcessingOre;
      ProcessingStage.Value = data.Economy.ProcessingStage;
      
      Subscribe();
    }

    private void Subscribe()
    {
      Money.Subscribe(_ => IsDirty = true);
      Ore.Subscribe(_ => IsDirty = true);
      RestOre.Subscribe(_ => IsDirty = true);
      ProcessingMoney.Subscribe(_ => IsDirty = true);
      ProcessingOre.Subscribe(_ => IsDirty = true);
      ProcessingStage.Subscribe(_ => IsDirty = true);
    }
  }
}