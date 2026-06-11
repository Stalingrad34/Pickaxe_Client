using System;
using System.Collections.Generic;
using Game.Scripts.Gameplay.ECS;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using UniRx;

namespace Game.Scripts.Infrastructure.Services
{
  public class PickaxesService : IService, IStorageProcessor
  {
    public bool IsDirty { get; private set; }
    public readonly ReactiveProperty<ulong> PickaxesNominal = new();
    
    private Dictionary<PickaxeType, int> _pickaxes = new();
    private IDisposable _pickaxesTimer;

    public void StartPickaxeTimer()
    {
      _pickaxesTimer = Observable
        .Timer(TimeSpan.FromSeconds(5))
        .Repeat()
        .Subscribe(_ => PickaxesPunchFire());
    }

    public void StopPickaxeTimer()
    {
      _pickaxesTimer.Dispose();
    }
    
    public void TryAddPickaxes(int count)
    {
      var database = ServiceProvider.Get<EconomyService>();
      var cost = GetPickaxeCost() * (ulong)count;
      if (cost > database.Money.Value)
        return;
      
      database.Money.Value -= cost;
      AddPickaxe(PickaxeType.Wood, count);
      RebuildPickaxes("player");
    }

    public void RebuildPickaxes(string ownerId)
    {
      ECSRunner.EcsEventWriter.RebuildPickaxes(ownerId, _pickaxes);
    }
    
    public ulong GetPickaxeCost()
    {
      if (PickaxesNominal.Value == 0)
        return 5;

      return PickaxesNominal.Value * 7 / 5 + 6;
    }

    private void PickaxesPunchFire()
    {
      ECSRunner.EcsEventWriter.PickaxesPunch("player");
    }
    
    private void AddPickaxe(PickaxeType type, int amount = 1)
    {
      _pickaxes[type] = GetCount(type) + amount;
      TryMergeAll();
      IsDirty = true;
    }

    private void TryMergeAll()
    {
      TryMerge(PickaxeType.Wood, PickaxeType.Stone);
      TryMerge(PickaxeType.Stone, PickaxeType.Cuprum);
    }

    private void TryMerge(PickaxeType from, PickaxeType to)
    {
      int count = GetCount(from);

      if (count < 3)
        return;

      int upgradedCount = count / 3;
      int remainder = count % 3;

      _pickaxes[from] = remainder;
      _pickaxes[to] = GetCount(to) + upgradedCount;
    }

    private int GetCount(PickaxeType type)
    {
      return _pickaxes.GetValueOrDefault(type, 0);
    }
    
    public void Save(SaveData data)
    {
      data.Pickaxes.PickaxesNominal = PickaxesNominal.Value;
      data.Pickaxes.Pickaxes = _pickaxes;
      
      IsDirty = false;
    }

    public void Load(SaveData data)
    {
      PickaxesNominal.Value = data.Pickaxes.PickaxesNominal;
      _pickaxes = data.Pickaxes.Pickaxes;
      
      Subscribe();
    }
    
    private void Subscribe()
    {
      PickaxesNominal.Subscribe(_ => IsDirty = true);
    }
  }
}