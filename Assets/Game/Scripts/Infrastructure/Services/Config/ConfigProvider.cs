using YG;

namespace Game.Scripts.Infrastructure.Services.Config
{
  public class ConfigProvider : IInitializableService
  {
    public bool EnableRewarded;
    public float ChestChance;
    public int AdsCooldown;
    
    public void Init()
    {
      EnableRewarded = YG2.TryGetFlagAsBool("enable_rewarded", out var enable ) && enable;
      ChestChance = YG2.TryGetFlagAsFloat("chest_chance", out var chance ) ? chance : 0.01f;
      AdsCooldown = YG2.TryGetFlagAsInt("ads_cooldown", out var cooldown ) ? cooldown : 60;
    }
  }
}