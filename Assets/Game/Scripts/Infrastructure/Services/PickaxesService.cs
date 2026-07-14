using System;
using System.Collections.Generic;
using System.Linq;
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
    public readonly ReactiveProperty<bool> CanMerge = new();
    public readonly ReactiveProperty<long> PickaxesPunchLastTime = new();
    
    private Dictionary<PickaxeType, int> _pickaxes = new();
    private IDisposable _pickaxesTimer;

    public void CheckRestOre()
    {
      var currentTime = ServiceProvider.Get<TimeProvider>().GetCurrentTimeStamp();
      var orePerSecond = 0;
      foreach (var (pickaxe, count) in _pickaxes)
      {
        var config = AssetProvider.GetPickaxeData(pickaxe);
        orePerSecond += config.oreConfig.miningCount * count;
      }
      
      var restSeconds = currentTime - PickaxesPunchLastTime.Value;
      ServiceProvider.Get<EconomyService>().AddRestOre(orePerSecond * (restSeconds / 5));
      
      PickaxesPunchLastTime.Value = currentTime;
    }

    public void StartPickaxeTimer()
    {
      CheckCanMarge();
      _pickaxesTimer = Observable
        .Timer(TimeSpan.FromSeconds(5))
        .Repeat()
        .Subscribe(_ => PickaxesPunchFire());
    }

    public void StopPickaxeTimer()
    {
      _pickaxesTimer.Dispose();
    }
    
    public void TryAddPickaxes(int count, bool autoMerge)
    {
      var database = ServiceProvider.Get<EconomyService>();
      var cost = GetPickaxeCost() * (ulong)count;
      if (cost > database.Money.Value)
        return;
      
      database.Money.Value -= cost;
      PickaxesNominal.Value += (ulong)count;
      AddPickaxe(PickaxeType.Wood, count);

      if (autoMerge || PickaxesCount() > 100)
        TryMergeAll();
      else
        RebuildPickaxes("player");
      
      CheckCanMarge();
    }

    private void CheckCanMarge()
    {
      CanMerge.Value = _pickaxes.Any(kvp => kvp.Value >= 3);
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
      PickaxesPunchLastTime.Value = ServiceProvider.Get<TimeProvider>().GetCurrentTimeStamp();
      ECSRunner.EcsEventWriter.PickaxesPunch("player");
    }
    
    private void AddPickaxe(PickaxeType type, int amount = 1)
    {
      _pickaxes[type] = GetCount(type) + amount;
      IsDirty = true;
    }

    public void TryMergeAll()
    {
      TryMerge(PickaxeType.Wood, PickaxeType.Stone);
      TryMerge(PickaxeType.Stone, PickaxeType.Copper);
      TryMerge(PickaxeType.Copper, PickaxeType.Iron);
      TryMerge(PickaxeType.Iron, PickaxeType.Titan);
      TryMerge(PickaxeType.Titan, PickaxeType.Wolfram);
      TryMerge(PickaxeType.Wolfram, PickaxeType.Crystal);
      TryMerge(PickaxeType.Crystal, PickaxeType.Obsidian);
      TryMerge(PickaxeType.Obsidian, PickaxeType.Emerald);
      TryMerge(PickaxeType.Emerald, PickaxeType.Sapphire);
      TryMerge(PickaxeType.Sapphire, PickaxeType.Rubin);
      TryMerge(PickaxeType.Rubin, PickaxeType.Carbon);
      TryMerge(PickaxeType.Carbon, PickaxeType.Diamond);
      RebuildPickaxes("player");
      CanMerge.Value = false;
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

    private int PickaxesCount()
    {
      return _pickaxes.Sum(kvp => kvp.Value);
    }
    
    public void Save(SaveData data)
    {
      data.Pickaxes.PickaxesNominal = PickaxesNominal.Value;
      data.Pickaxes.Pickaxes = _pickaxes;
      data.Pickaxes.PickaxesPunchLastTime = PickaxesPunchLastTime.Value;
      
      IsDirty = false;
    }

    public void Load(SaveData data)
    {
      PickaxesNominal.Value = data.Pickaxes.PickaxesNominal;
      _pickaxes = data.Pickaxes.Pickaxes;
      PickaxesPunchLastTime.Value = data.Pickaxes.PickaxesPunchLastTime;
      
      Subscribe();
    }
    
    private void Subscribe()
    {
      PickaxesNominal.Subscribe(_ => IsDirty = true);
      PickaxesPunchLastTime.Subscribe(_ => IsDirty = true);
    }
  }
}