using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using UniRx;

namespace Game.Scripts.Infrastructure.Services
{
  public class EconomyService : IService, IStorageProcessor
  {
    public bool IsDirty { get; private set; }
    
    public readonly ReactiveProperty<ulong> Money = new();
    public readonly ReactiveProperty<ulong> Ore = new();
    public readonly ReactiveProperty<ulong> ProcessingMoney = new();
    public readonly ReactiveProperty<ulong> ProcessingOre = new();

    public void Save(SaveData data)
    {
      data.Economy.Money = Money.Value;
      data.Economy.Ore = Ore.Value;
      data.Economy.ProcessingMoney = ProcessingMoney.Value;
      data.Economy.ProcessingOre = ProcessingOre.Value;

      IsDirty = false;
    }

    public void Load(SaveData data)
    {
      Money.Value = data.Economy.Money;
      Ore.Value = data.Economy.Ore;
      ProcessingMoney.Value = data.Economy.ProcessingMoney;
      ProcessingOre.Value = data.Economy.ProcessingOre;
      
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