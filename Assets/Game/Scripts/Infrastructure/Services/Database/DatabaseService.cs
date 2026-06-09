using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Infrastructure.Extensions;
using UniRx;

namespace Game.Scripts.Infrastructure.Services.Database
{
  public class DatabaseService : IInitializableService
  {
    public readonly ReactiveProperty<string> PlayerName = new();
    public readonly ReactiveProperty<ulong> Money = new();
    public readonly ReactiveProperty<ulong> Ore = new();
    public readonly ReactiveProperty<ulong> ProcessingMoney = new();
    public readonly ReactiveProperty<ulong> ProcessingOre = new();
    public readonly ReactiveProperty<ulong> PickaxesNominal = new();
    public readonly ReactiveCollection<PickaxeData> Pickaxes = new(new List<PickaxeData>());
    
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
      return new PrefsDatabaseProcessor();//new MySqlDatabaseProcessor(_connectConfig);
    }

    private void InitSaveProcessor()
    {
      Money.Subscribe(_ => _canSave = true);
      Ore.Subscribe(_ => _canSave = true);
      ProcessingMoney.Subscribe(_ => _canSave = true);
      ProcessingOre.Subscribe(_ => _canSave = true);
      PickaxesNominal.Subscribe(_ => _canSave = true);
      Pickaxes.SubscribeAdd((_, _) => _canSave = true);
      
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