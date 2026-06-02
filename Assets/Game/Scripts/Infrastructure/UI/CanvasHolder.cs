using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class CanvasHolder : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GraphicRaycaster raycaster;
        
        public void SetCamera(Camera uiCamera)
        {
            canvas.worldCamera = uiCamera;
        }

        public void SetActiveRaycaster(bool isActive)
        {
            raycaster.enabled = isActive;
        }
    }
}