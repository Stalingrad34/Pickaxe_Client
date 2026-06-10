using Game.Scripts.Infrastructure.Services.Storage.Data;

namespace Game.Scripts.Infrastructure.Services.Storage
{
  public interface IStorageProcessor
  {
    bool IsDirty { get; }
    void Save(SaveData data);
    void Load(SaveData data);
  }
}