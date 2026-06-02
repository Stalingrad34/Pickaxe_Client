using Game.Scripts.Infrastructure.Custom;
using UnityEditor;
using UnityEditor.UI;

namespace Game.Scripts.Editor
{
    [CustomEditor(typeof(CustomButton))]
    public class CustomButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            CustomButton customButton = (CustomButton)target;
            base.OnInspectorGUI();
            customButton.text = EditorGUILayout.ObjectField("Text", customButton.text, typeof(CustomText), true) as CustomText;
            customButton.sound = EditorGUILayout.TextField("Sound", customButton.sound);
        }
    }
}
