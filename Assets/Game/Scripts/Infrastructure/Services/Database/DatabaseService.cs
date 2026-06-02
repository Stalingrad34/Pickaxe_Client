using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Extensions;
using UniRx;

namespace Game.Scripts.Infrastructure.Services.Database
{
  public class DatabaseService : IInitializableService
  {
    public readonly ReactiveProperty<string> PlayerName = new();
    public readonly ReactiveProperty<string> Weapon = new();
    public readonly ReactiveProperty<string> Face = new();
    public readonly ReactiveProperty<string> Body = new();
    public readonly ReactiveProperty<int> Slaps = new();
    public readonly ReactiveCollection<string> Inventory = new();
    
    private IDatabaseProcessor _databaseProcessor;
    private bool _canSave;
    private CancellationTokenSource _saveTokenSource;
    private ConnectConfig _connectConfig;

    public DatabaseService(ConnectConfig connectConfig)
    {
      _connectConfig = connectConfig;
    }

    public async UniTask Init(CancellationToken token)
    {
      _databaseProcessor = GetDatabaseProcessor();
      await _databaseProcessor.Load(this);
      
      InitSaveProcessor();
    }

    private IDatabaseProcessor GetDatabaseProcessor()
    {
      return new MySqlDatabaseProcessor(_connectConfig);
    }

    private void InitSaveProcessor()
    {
      PlayerName.Subscribe(_ => _canSave = true);
      Weapon.Subscribe(_ => _canSave = true);
      Face.Subscribe(_ => _canSave = true);
      Body.Subscribe(_ => _canSave = true);
      Slaps.Subscribe(_ => _canSave = true);
      Inventory.SubscribeAdd((_, _) => _canSave = true);
      
      StartSaveProcessor().Forget();
    }

    private async UniTaskVoid StartSaveProcessor()
    {
      _saveTokenSource = new CancellationTokenSource();

      while (true)
      {
        if (_canSave)
        {
          await _databaseProcessor.Save(this);
          _canSave = false;
        }
        
        await UniTask.WaitForEndOfFrame(_saveTokenSource.Token).SuppressCancellationThrow();
        if (_saveTokenSource.IsCancellationRequested)
        {
          return;
        }
      }
    }
  }
}