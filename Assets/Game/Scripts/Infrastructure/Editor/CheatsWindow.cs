using Game.Scripts.Infrastructure.Services;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Editor
{
  public class CheatsWindow : OdinEditorWindow
  {
    [ButtonGroup("Money")]
    [Button("Money +100")]
    public void AddMoney100()
    {
      ServiceProvider.Get<EconomyService>().Money.Value += 100;
    }
    
    [ButtonGroup("Money")]
    [Button("Money +1000")]
    public void AddMoney1000()
    {
      ServiceProvider.Get<EconomyService>().Money.Value += 1000;
    }
    
    [Button]
    public void AddOre()
    {
      ServiceProvider.Get<EconomyService>().Ore.Value += 100;
    }
    
    [HorizontalGroup("LangButtons")]
    [Button("Ru", ButtonSizes.Medium)]
    [GUIColor(nameof(GetRuButtonColor))]
    private void SelectRu()
    {
      if (!ServiceProvider.Has<LocalizationService>())
        return;
      
      ServiceProvider.Get<LocalizationService>().ChangeLanguage("ru");
    }

    [HorizontalGroup("LangButtons")]
    [Button("En", ButtonSizes.Medium)]
    [GUIColor(nameof(GetEnButtonColor))]
    private void SelectEn()
    {
      if (!ServiceProvider.Has<LocalizationService>())
        return;
      
      ServiceProvider.Get<LocalizationService>().ChangeLanguage("en");
    }
    
    [HorizontalGroup("LangButtons")]
    [Button("Tr", ButtonSizes.Medium)]
    [GUIColor(nameof(GetTrButtonColor))]
    private void SelectTr()
    {
      if (!ServiceProvider.Has<LocalizationService>())
        return;
      
      ServiceProvider.Get<LocalizationService>().ChangeLanguage("tr");
    }
    
    [HorizontalGroup("LangButtons")]
    [Button("Pt", ButtonSizes.Medium)]
    [GUIColor(nameof(GetPtButtonColor))]
    private void SelectBr()
    {
      if (!ServiceProvider.Has<LocalizationService>())
        return;
      
      ServiceProvider.Get<LocalizationService>().ChangeLanguage("pt");
    }

    private Color GetRuButtonColor()
    {
      if (!ServiceProvider.Has<LocalizationService>())
        return Color.white;
      
      return ServiceProvider.Get<LocalizationService>().CurrentLanguage == "ru"
        ? Color.green
        : Color.white;
    }

    private Color GetEnButtonColor()
    {
      if (!ServiceProvider.Has<LocalizationService>())
        return Color.white;
      
      return ServiceProvider.Get<LocalizationService>().CurrentLanguage == "en"
        ? Color.green
        : Color.white;
    }
    
    private Color GetTrButtonColor()
    {
      if (!ServiceProvider.Has<LocalizationService>())
        return Color.white;
      
      return ServiceProvider.Get<LocalizationService>().CurrentLanguage == "tr"
        ? Color.green
        : Color.white;
    }
    
    private Color GetPtButtonColor()
    {
      if (!ServiceProvider.Has<LocalizationService>())
        return Color.white;
      
      return ServiceProvider.Get<LocalizationService>().CurrentLanguage == "br"
        ? Color.green
        : Color.white;
    }
    
    public static void OpenWindow()
    {
      var window = GetWindow<CheatsWindow>();
      window.titleContent = new GUIContent("Cheats");
      window.minSize = new Vector2(800, 600);
      window.Show();
    }
  }
}