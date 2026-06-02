using Game.Scripts.Infrastructure.Custom;
using TMPro;
using UnityEditor;
using TextEditor = UnityEditor.UI.TextEditor;

namespace Game.Scripts.Editor
{
  public class CustomTextEditor : TextEditor
  {
    [MenuItem("CONTEXT/TextMeshProUGUI/Replace Custom Text")]
    private static void ReplaceCustomText(MenuCommand command)
    {
      if (command.context is TextMeshProUGUI text)
      {
        var textOld = text.text;
        var fontAsset = text.font;
        var fontSize = text.fontSize;
        var alignment = text.alignment;
        var material = text.fontSharedMaterial;
        var color = text.color;
        
        var gameObject = text.gameObject;
        DestroyImmediate(text);
        var customText = gameObject.AddComponent<CustomText>();

        customText.text = textOld;
        customText.font = fontAsset;
        customText.fontSize = fontSize;
        customText.alignment = alignment;
        customText.fontSharedMaterial = material;
        customText.color = color;
      }
    }
  }
}