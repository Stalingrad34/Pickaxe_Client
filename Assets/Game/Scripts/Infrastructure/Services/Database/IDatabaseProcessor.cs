using Cysharp.Threading.Tasks;

namespace Game.Scripts.Infrastructure.Services.Database
{
  public interface IDatabaseProcessor
  {
    UniTask Load(DatabaseService database);
    UniTask Save(DatabaseService database);
  }
}