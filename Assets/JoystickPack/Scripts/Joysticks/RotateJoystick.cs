using Game.Scripts.Infrastructure;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JoystickPack.Scripts.Joysticks
{
  public class RotateJoystick : MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler
  {
    [SerializeField] private float sensitivity = 1;
    public Vector2 delta;
    private Vector2 _startPoint;

    private void Update()
    {
      delta = Vector2.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      //SetCursorLocked(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
      delta = eventData.delta * sensitivity;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      SetCursorLocked(false);
    }
    
    protected void SetCursorLocked(bool isLocked)
    {
      if (!Platform.IsStandaloneWebGL() && !Platform.IsEditor()) 
        return;
      
      Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
      Cursor.visible = !isLocked;
    }
  }
}