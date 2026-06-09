using Game.Scripts.Infrastructure.Services;
using UnityEngine;

namespace Game.Scripts.Gameplay.OreProcessing
{
  public class OreProcessingService : IService
  {
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
  }
}