using UnityEngine;

namespace Game.Scripts.Infrastructure.Custom
{
    [CreateAssetMenu(menuName = "Data/Custom Button/Style")]
    public class CustomButtonStyle : ScriptableObject
    {
        public Color textColor;
        public Sprite sprite;
    }
}