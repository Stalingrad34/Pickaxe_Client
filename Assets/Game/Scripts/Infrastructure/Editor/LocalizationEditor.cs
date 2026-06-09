using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Editor
{
  public class LocalizationEditor : OdinEditorWindow
  {
    [OnInspectorInit]
    public void Init()
    {
      if (LocaleList.Count > 0)
        return;
      
      var json = Resources.Load<TextAsset>("Localization");
      var localization = json != null 
        ? JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json.text)
        : new Dictionary<string, Dictionary<string, string>>();
      
      foreach (var (key, language) in localization)
      {
        var locale = new KeyValueLocale() {Key = key, Russian = language["ru"], English = language["en"]};
        LocaleList.Add(locale);
      }
    }
    
    [Button]
    public void Save()
    {
      var localization = new Dictionary<string, Dictionary<string, string>>();
      foreach (var locale in LocaleList)
      {
        var language = new Dictionary<string, string>
        {
          ["ru"] = locale.Russian,
          ["en"] = locale.English
        };
        
        localization.Add(locale.Key, language);
      }
      
      var json = JsonConvert.SerializeObject(localization);
      var path = "Assets/Game/Resources/Localization.json";
      File.WriteAllText(path, json);
      AssetDatabase.ImportAsset(path);
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
    }
    
    [TableList]
    public List<KeyValueLocale> LocaleList = new ();

    [Serializable]
    public class KeyValueLocale
    {
      [TableColumnWidth(20)]
      public string Key;
      public string Russian;
      public string English;
    }

    public static void OpenWindow()
    {
      var window = GetWindow<LocalizationEditor>();
      window.titleContent = new GUIContent("Localization Editor");
      window.minSize = new Vector2(800, 600);
      window.Show();
    }

  }
}