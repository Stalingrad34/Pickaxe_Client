using Game.Scripts.Infrastructure.Services;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Editor
{
  public class CheatsWindow : OdinEditorWindow
  {
    [Button]
    public void AddMoney()
    {
      ServiceProvider.Get<EconomyService>().Money.Value += 100;
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