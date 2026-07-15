using Game.Scripts.Infrastructure.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game.Scripts.Editor
{
  public class Tools
  {
    [MenuItem("Game/Play", false, 1)]
    static void Play()
    {
      EditorSceneManager.OpenScene("Assets/Game/Scenes/Bootstrap.unity");
      EditorApplication.isPlaying = true;
    }

    [MenuItem("Game/Clear and Play", false, 2)]
    static void ClearAndPlay()
    {
      PlayerPrefs.DeleteAll();
      EditorSceneManager.OpenScene("Assets/Game/Scenes/Bootstrap.unity");
      EditorApplication.isPlaying = true;
    }
    
    [MenuItem("Tools/Cheats", false, 3)]
    static void OpenCheats()
    {
      CheatsWindow.OpenWindow();
    }
    
    [MenuItem("Game/OpenLocalization", false, 4)]
    static void OpenLocalization()
    {
      LocalizationEditor.OpenWindow();
    }
  }
}