using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services
{
  public class LocalizationService : IInitializableService, IStorageProcessor
  {
    public bool IsDirty { get; private set; }
    public readonly ReactiveCommand LanguageChanged = new();

    private Dictionary<string, Dictionary<string, string>> _localizations;

    public string CurrentLanguage => _currentLanguage;
    private string _currentLanguage;

    public async UniTask Init(CancellationToken token)
    {
      var json = await Resources.LoadAsync<TextAsset>("Localization") as TextAsset;
      _localizations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json.text);
    }
    
    public void ChangeLanguage(string locale)
    {
      _currentLanguage = locale;
      IsDirty = true;
      LanguageChanged.Execute();
    }

    public string GetLocalizedText(string text)
    {
      return _localizations.TryGetValue(text, out var localizations) ? localizations[_currentLanguage] : text;
    }
    
    public void Save(SaveData data)
    {
      data.Player.Language = _currentLanguage;
      IsDirty = false;
    }

    public void Load(SaveData data)
    {
      _currentLanguage = data.Player.Language;
    }
  }
}