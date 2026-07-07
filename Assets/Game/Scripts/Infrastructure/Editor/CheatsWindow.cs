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
    
    public static void OpenWindow()
    {
      var window = GetWindow<CheatsWindow>();
      window.titleContent = new GUIContent("Cheats");
      window.minSize = new Vector2(800, 600);
      window.Show();
    }
  }
}