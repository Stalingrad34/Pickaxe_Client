using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services
{
  public class SettingsProvider : IInitializableService
  {
    private string SOUND_KEY = "sound_value";
    private string SENSITIVITY_KEY = "sensitivity_value";

    public readonly ReactiveProperty<float> SoundValue = new();
    public readonly ReactiveProperty<float> SensitivityValue = new();

    private bool _canSave;
    private CancellationTokenSource _saveTokenSource;
    
    public async UniTask Init(CancellationToken token)
    {
      SoundValue.Value = PlayerPrefs.GetFloat(SOUND_KEY, 1);
      SensitivityValue.Value = PlayerPrefs.GetFloat(SENSITIVITY_KEY, 1);
      
      SoundValue.Subscribe(_ => _canSave = true);
      SensitivityValue.Subscribe(_ => _canSave = true);
      
      StartSaveProcessor().Forget();
      await UniTask.Yield(cancellationToken:token).SuppressCancellationThrow();
    }

    private void Save()
    {
      PlayerPrefs.SetFloat(SOUND_KEY, SoundValue.Value);
      PlayerPrefs.SetFloat(SENSITIVITY_KEY, SensitivityValue.Value);
    }

    private async UniTaskVoid StartSaveProcessor()
    {
      _saveTokenSource = new CancellationTokenSource();

      while (true)
      {
        if (_canSave)
        {
          Save();
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