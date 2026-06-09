using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Game.Scripts.Infrastructure.Services
{
  public class LocalizationService : IService
  {
    public readonly ReactiveCommand LanguageChanged = new();

    public LocalizationService()
    {
      LocalizationSettings.SelectedLocaleChanged += LocaleChanged;
    }

    private void LocaleChanged(Locale locale)
    {
      LanguageChanged.Execute();
    }

    public async UniTask<string> GetLocalizedText(string key)
    {
      var table = await LocalizationSettings.StringDatabase.GetTableAsync("Localization");
      if (table == null)
      {
        return key;
      }

      var entry = table.GetEntry(key);
      if (entry == null)
      {
        return key;
      }

      var value = entry.GetLocalizedString();
      return string.IsNullOrEmpty(value) ? key : value;
    }
  }
}