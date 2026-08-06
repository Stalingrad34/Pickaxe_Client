using System.Collections.Generic;
using Game.Scripts.Gameplay.Chest;
using YG;

namespace Game.Scripts.Infrastructure.Services.Config
{
  public class ConfigProvider : IInitializableService
  {
    public bool EnableRewarded;
    public float ChestChance;
    public int AdsCooldown;
    public readonly Dictionary<ChestType, int> ChestChances = new();
    
    public void Init()
    {
      EnableRewarded = YG2.TryGetFlagAsBool("enable_rewarded", out var enable ) && enable;
      ChestChance = YG2.TryGetFlagAsFloat("chest_chance", out var chance ) ? chance : 0.01f;
      AdsCooldown = YG2.TryGetFlagAsInt("ads_cooldown", out var cooldown ) ? cooldown : 60;
      ChestChances[ChestType.Wood] = YG2.TryGetFlagAsInt("chest_chance_wood", out var woodChance ) ? woodChance : 0;
      ChestChances[ChestType.Shiny] = YG2.TryGetFlagAsInt("chest_chance_shiny", out var shinyChance ) ? shinyChance : 0;
      ChestChances[ChestType.Rare] = YG2.TryGetFlagAsInt("chest_chance_rare", out var rareChance ) ? rareChance : 0;
    }
  }
}