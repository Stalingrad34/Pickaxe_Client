using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
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
    public readonly ReactiveProperty<int> ProcessingStage = new();
    public readonly ReactiveCommand<PickupTextData> PickupTextCommand = new();

    public void AddOre(ulong amount, Color pickupTextColor)
    {
      Ore.Value += amount;
      var pickupData = new PickupTextData()
      {
        Text = $"+{amount}",
        Color = pickupTextColor
      };
      PickupTextCommand.Execute(pickupData);
    }
    
    public void CollectMoney()
    {
      Money.Value += ProcessingMoney.Value;
      ProcessingMoney.Value = 0;
    }
    
    public void IncreaseStage(ulong cost)
    {
      if (cost > Money.Value)
        return;
      
      Money.Value -= cost;
      ProcessingStage.Value++;
    }

    public void Save(SaveData data)
    {
      data.Economy.Money = Money.Value;
      data.Economy.Ore = Ore.Value;
      data.Economy.ProcessingMoney = ProcessingMoney.Value;
      data.Economy.ProcessingOre = ProcessingOre.Value;
      data.Economy.ProcessingStage = ProcessingStage.Value;

      IsDirty = false;
    }

    public void Load(SaveData data)
    {
      Money.Value = data.Economy.Money;
      Ore.Value = data.Economy.Ore;
      ProcessingMoney.Value = data.Economy.ProcessingMoney;
      ProcessingOre.Value = data.Economy.ProcessingOre;
      ProcessingStage.Value = data.Economy.ProcessingStage;
      
      Subscribe();
    }

    private void Subscribe()
    {
      Money.Subscribe(_ => IsDirty = true);
      Ore.Subscribe(_ => IsDirty = true);
      ProcessingMoney.Subscribe(_ => IsDirty = true);
      ProcessingOre.Subscribe(_ => IsDirty = true);
    }
  }
}