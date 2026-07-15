using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services;

namespace Game.Scripts.Infrastructure.Extensions
{
  public static class CommonExtensions
  {
    public static string Localize(this string text)
    {
      return ServiceProvider.Get<LocalizationService>().GetLocalizedText(text);
    }
  }
}