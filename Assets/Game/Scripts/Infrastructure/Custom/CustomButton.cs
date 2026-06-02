using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.Infrastructure.Custom
{
    [RequireComponent(typeof(Image))]
    public class CustomButton : Button
    {
        public CustomText text;
        public string sound;
        public RectTransform RectTransform => transform as RectTransform;
        public bool WasPressed => gameObject && IsPressed();

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (text == null)
            {
                text = GetComponentInChildren<CustomText>();
            }
        }
#endif
        public override void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log($"{GetPath(transform)} clicked");
            base.OnPointerDown(eventData);
            PlayClickSound();
        }

        public void SetupStyle(CustomButtonStyle style)
        {
            image.sprite = style.sprite;
            text.color = style.textColor;
        }

        private void PlayClickSound()
        {
            AudioController.Instance.PlayAudioClipFromSoundMap(!string.IsNullOrEmpty(sound) ? sound : "Click");
        }

        private string GetPath(Transform transform)
        {
            if (!transform.parent)
            {
                return transform.name;

            }
            return GetPath(transform.parent) + "/" + transform.name;
        }
    }
}
