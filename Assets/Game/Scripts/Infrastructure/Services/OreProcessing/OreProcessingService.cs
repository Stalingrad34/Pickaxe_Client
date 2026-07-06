using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.OreProcessing
{
  public class OreProcessingService : IService
  {
    private readonly EconomyService _economy;
    private int _processingStage;
    private IDisposable _processTimer;

    public OreProcessingService(EconomyService economy)
    {
      _economy = economy;
      _economy.ProcessingStage.Subscribe(ProcessingStageChanged);
    }
    
    public void ProcessOre()
    {
      _economy.ProcessingOre.Value += _economy.Ore.Value;
      _economy.Ore.Value = 0;

      if (_processTimer == null)
        StartProcessTimer();
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

    private int GetOrePerSecond(int stage)
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
      _processTimer = Observable.Timer(TimeSpan.FromSeconds(1)).Repeat().Subscribe(_ =>
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

    private void ProcessingStageChanged(int stage)
    {
      _processingStage = stage;
    }
  }
}