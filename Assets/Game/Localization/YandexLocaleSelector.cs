using BitGames.Bits;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using YG;

namespace Game.Localization
{
  public class YandexLocaleSelector : IStartupLocaleSelector
  {
    public Locale GetStartupLocale(ILocalesProvider availableLocales)
    {
      if (!Platform.IsWebGL())
        return null;
      
      var locale = YG2.envir.language;
      return availableLocales.GetLocale(new LocaleIdentifier(locale));
    }
  }
}