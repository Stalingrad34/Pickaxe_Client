using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.Database;
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
      ServiceProvider.Get<DatabaseService>().Money.Value += 100;
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