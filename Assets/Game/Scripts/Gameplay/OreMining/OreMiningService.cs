using Game.Scripts.Infrastructure.Services;

namespace Game.Scripts.Gameplay.OreMining
{
  public class OreMiningService : IService
  {
    public ulong GetPickaxeCost(ulong pickaxeCount)
    {
      if (pickaxeCount == 0)
        return 5;

      return pickaxeCount * 7 / 5 + 6;
    }
  }
}