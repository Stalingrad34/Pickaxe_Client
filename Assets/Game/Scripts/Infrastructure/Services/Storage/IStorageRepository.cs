using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services.Storage.Data;

namespace Game.Scripts.Infrastructure.Services.Storage
{
  public interface IStorageRepository
  {
    UniTask Save(SaveData data);
    UniTask<SaveData> Load();
  }
}