using System;
using Game.Scripts.Infrastructure.Services;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Gameplay.Environment
{
    public class InstantProcessOreButton : MonoBehaviour, IPointerClickHandler
    {
        public Texture2D hoverCursor;
        public Vector2 hoverCursorHotspot = new Vector2(26f, 6f);
        private Vector3 _startScale;

        private void Awake()
        {
            _startScale = transform.localScale;
            ServiceProvider.Get<EconomyService>().ProcessingOre.Subscribe(ProcessingOreChanged).AddTo(gameObject);
        }

        private void OnDisable()
        {
            ResetCursor();
        }

        private void ProcessingOreChanged(ulong amount)
        {
            gameObject.SetActive(amount > 0);
        }

        private void OnMouseEnter()
        {
            if (hoverCursor != null && hoverCursor.isReadable)
            {
                Cursor.SetCursor(
                    hoverCursor,
                    hoverCursorHotspot,
                    CursorMode.Auto
                );
            }

            transform.localScale = _startScale * 1.05f;
        }

        private void OnMouseExit()
        {
            ResetCursor();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ResetCursor();
            ServiceProvider.Get<OreProcessingService>().InstantProcessOreByAds();
        }
        
        private void ResetCursor()
        {
            Cursor.SetCursor(
                null,
                Vector2.zero,
                CursorMode.Auto
            );

            transform.localScale = _startScale;
        }
    }
}
