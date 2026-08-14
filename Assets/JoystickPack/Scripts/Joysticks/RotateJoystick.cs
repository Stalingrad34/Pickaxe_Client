using Game.Scripts.Infrastructure;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Joystick = JoystickPack.Scripts.Base.Joystick;

namespace JoystickPack.Scripts.Joysticks
{
  public class RotateJoystick : MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler, ICanvasRaycastFilter
  {
    [SerializeField] private Joystick moveJoystick;
    [SerializeField] private float sensitivity = 1;
    [SerializeField] private LayerMask clickThroughMask = ~0;
    public Vector2 delta;
    public float zoom;
    private Vector2 _startPoint;
    private Camera _camera;

    private void Awake()
    {
      _camera = Camera.main;
    }

    private void Update()
    {
      delta = Vector2.zero;
      zoom = 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      //SetCursorLocked(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (IsTwoOrMoreTouches() && !moveJoystick.IsBusy)
      {
        zoom = GetTouchZoom();
        return;
      }
     
      delta = eventData.delta * sensitivity;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      SetCursorLocked(false);
    }
    
    private bool IsTwoOrMoreTouches()
    {
      var touchscreen = Touchscreen.current;
      if (touchscreen == null)
        return false;

      var pressedTouches = 0;

      foreach (var touch in touchscreen.touches)
      {
        if (!touch.press.isPressed)
          continue;

        pressedTouches++;

        if (pressedTouches >= 2)
          return true;
      }

      return false;
    }
    
    protected void SetCursorLocked(bool isLocked)
    {
      if (!Platform.IsStandaloneWebGL() && !Platform.IsEditor()) 
        return;
      
      Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
      Cursor.visible = !isLocked;
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
      var cam = _camera != null ? _camera : Camera.main;
      if (cam == null)
        return true;

      var ray = cam.ScreenPointToRay(screenPoint);

      if (!Physics.Raycast(ray, out var hit, 1000f, clickThroughMask))
        return true;

      return ExecuteEvents.GetEventHandler<IPointerClickHandler>(hit.collider.gameObject) == null;
    }
    
    private float GetTouchZoom()
    {
      var touchscreen = Touchscreen.current;
      if (touchscreen == null)
        return 0f;

      TouchControl firstTouch = null;
      TouchControl secondTouch = null;

      foreach (var touch in touchscreen.touches)
      {
        if (!touch.press.isPressed)
          continue;

        if (firstTouch == null)
          firstTouch = touch;
        else
        {
          secondTouch = touch;
          break;
        }
      }

      if (firstTouch == null || secondTouch == null)
        return 0f;

      var firstPosition = firstTouch.position.ReadValue();
      var secondPosition = secondTouch.position.ReadValue();

      var firstPreviousPosition = firstPosition - firstTouch.delta.ReadValue();
      var secondPreviousPosition = secondPosition - secondTouch.delta.ReadValue();

      var currentDistance = Vector2.Distance(firstPosition, secondPosition);
      var previousDistance = Vector2.Distance(firstPreviousPosition, secondPreviousPosition);

      var distanceDelta = currentDistance - previousDistance;

      if (Mathf.Abs(distanceDelta) < 2)
        return 0f;

      return Mathf.Clamp(-distanceDelta * 0.02f, -1f, 1f);
    }
  }
}