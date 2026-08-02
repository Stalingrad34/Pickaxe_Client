using System;
using Game.Scripts.Infrastructure.Services.Storage;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Infrastructure.Services
{
  public class OreProcessingService : IService, IDisposable
  {
    private const int ENABLE_MULTIPLIER_PICKAXES = 5;
    
    public readonly ReactiveProperty<float> ProcessingMultiplier = new();
    public readonly ReactiveProperty<int> MultiplierTimerSeconds = new();
    public readonly ReactiveProperty<bool> MultiplierEnabled = new();
    
    private readonly EconomyService _economy;
    private readonly PickaxesService _pickaxesService;
    private int _processingStage;
    private IDisposable _processTimer;
    private IDisposable _multiplierTimer;
    private IDisposable _subscribePickaxes;

    public OreProcessingService(EconomyService economy, PickaxesService pickaxesService)
    {
      _economy = economy;
      _pickaxesService = pickaxesService;
      _economy.ProcessingStage.Subscribe(ProcessingStageChanged);
    }
    
    public void ProcessOre()
    {
      var multiplier = MultiplierEnabled.Value ? (double)ProcessingMultiplier.Value : 1;
      _economy.ProcessingOre.Value += (ulong)(_economy.Ore.Value * multiplier);
      _economy.Ore.Value = 0;

      if (_processTimer == null)
        StartProcessTimer();
    }

    public void StartTimers()
    {
      if (_economy.ProcessingOre.Value > 0)
        StartProcessTimer();

      if (_pickaxesService.PickaxesNominal.Value >= ENABLE_MULTIPLIER_PICKAXES)
        StartMultiplierTimer();
      else
        _subscribePickaxes = _pickaxesService.PickaxesNominal.Subscribe(PickaxesNominalChanged);
    }
    
    public OrePrecessingData GetOrePrecessingData(int stage)
    {
      if (stage <= 0)
      {
        return new OrePrecessingData
        {
          OreCount = 0,
          UpgradeCost = 0
        };
      }

      return new OrePrecessingData
      {
        OreCount = GetOrePerSecond(stage),
        UpgradeCost = GetUpgradeCost(stage)
      };
    }

    public int GetOrePerSecond(int stage)
    {
      int ore = 1;

      for (int i = 0; i < stage; i++)
      {
        ore += Mathf.FloorToInt(0.12f * i * i + 0.5f * i + 2f);
      }

      return ore;
    }

    private int GetUpgradeCost(int stage)
    {
      int targetOre = GetOrePerSecond(stage);

      return Mathf.RoundToInt(0.45f * targetOre * Mathf.Log(targetOre, 2f));
    }

    private void StartProcessTimer()
    {
      _processTimer = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ =>
      {
        if (_economy.ProcessingOre.Value > 0)
        {
          var processedOre = Math.Min((ulong)GetOrePerSecond(_processingStage), _economy.ProcessingOre.Value);
          _economy.ProcessingOre.Value -= processedOre;
          _economy.ProcessingMoney.Value += processedOre;
        }
        else
        {
          _processTimer.Dispose();
          _processTimer = null;
        }
      });
    }
    
    private void StartMultiplierTimer()
    {
      MultiplierEnabled.Value = true;
      ProcessingMultiplier.Value = GetMultiplier();
      MultiplierTimerSeconds.Value = 30;
      _multiplierTimer = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ =>
      {
        MultiplierTimerSeconds.Value--;
        if (MultiplierTimerSeconds.Value > 0)
          return;

        MultiplierTimerSeconds.Value = 30;
        ProcessingMultiplier.Value = GetMultiplier();
      });
    }

    private void ProcessingStageChanged(int stage)
    {
      _processingStage = stage;
    }

    private void PickaxesNominalChanged(ulong count)
    {
      if (count >= ENABLE_MULTIPLIER_PICKAXES)
      {
        _subscribePickaxes.Dispose();
        StartMultiplierTimer();
      }
    }

    private float GetMultiplier()
    {
      var random = Random.Range(0.5f, 1.5f);
      return (float)Math.Round(random, 1);
    }

    public void Dispose()
    {
      _processTimer?.Dispose();
      _multiplierTimer?.Dispose();
    }
  }
}