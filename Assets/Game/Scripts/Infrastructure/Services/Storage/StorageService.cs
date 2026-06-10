using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services.Storage.Data;

namespace Game.Scripts.Infrastructure.Services.Storage
{
  public class StorageService : IInitializableService
  {
    private readonly List<IStorageProcessor> _processors = new();
    private readonly IStorageRepository _repository;
    private CancellationTokenSource _saveTokenSource;

    public StorageService()
    {
      _repository = GetStorageRepository();
    }

    public async UniTask Init(CancellationToken token)
    {
      var database = await _repository.Load();
      foreach (var processor in _processors)
        processor.Load(database);

      StartSaveProcessor().Forget();
    }

    public StorageService AddProcessor(IStorageProcessor processor)
    {
      _processors.Add(processor);
      return this;
    }

    private IStorageRepository GetStorageRepository()
    {
      return new PrefsStorageProcessor();
    }
    
    private async UniTaskVoid StartSaveProcessor()
    {
      _saveTokenSource = new CancellationTokenSource();

      while (true)
      {
        if (HasDirty())
        {
          var saveData = new SaveData();
          foreach (var processor in _processors)
            processor.Save(saveData);
          
          await _repository.Save(saveData);
        }
        
        await UniTask.WaitForEndOfFrame(_saveTokenSource.Token).SuppressCancellationThrow();
        if (_saveTokenSource.IsCancellationRequested)
        {
          return;
        }
      }
    }

    private bool HasDirty()
    {
      return _processors.Any(p => p.IsDirty);
    }
  }
}