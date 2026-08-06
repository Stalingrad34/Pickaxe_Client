using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using YG;

namespace Game.Scripts.Infrastructure.Services.Storage
{
  public class YandexStorageProcessor : IStorageRepository
  {
    public async UniTask Save(SaveData data)
    {
      YG2.saves.SaveData = data;
      YG2.SaveProgress();
      await UniTask.Yield();
    }

    public async UniTask<SaveData> Load()
    {
      await UniTask.Yield();
      return YG2.saves.SaveData ?? new SaveData();
    }
  }
}