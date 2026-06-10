using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using UniRx;

namespace Game.Scripts.Infrastructure.Services
{
  public class PlayerService : IService, IStorageProcessor
  {
    public bool IsDirty { get; private set; }
    public readonly ReactiveProperty<string> PlayerName = new();
    
    public void Save(SaveData data)
    {
      data.Player.PlayerName = PlayerName.Value;

      IsDirty = false;
    }

    public void Load(SaveData data)
    {
      PlayerName.Value = data.Player.PlayerName;
      
      Subscribe();
    }
    
    private void Subscribe()
    {
      PlayerName.Subscribe(_ => IsDirty = true);
    }
  }
}