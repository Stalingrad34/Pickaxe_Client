using Game.Scripts.Gameplay.ECS;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.Database;

namespace Game.Scripts.Gameplay.OreMining
{
  public class OreMiningService : IService
  {
    public void TryAddPickaxes(int count)
    {
      var database = ServiceProvider.Get<DatabaseService>();
      var cost = GetPickaxeCost((ulong)count);
      if (cost > database.Money.Value)
        return;
      
      database.Money.Value -= cost;
      for (int i = 0; i < count; i++)
      {
        ECSRunner.EcsEventWriter.AddPickaxe("");
      }
    }
    
    public ulong GetPickaxeCost(ulong pickaxeCount)
    {
      if (pickaxeCount == 0)
        return 5;

      return pickaxeCount * 7 / 5 + 6;
    }
  }
}