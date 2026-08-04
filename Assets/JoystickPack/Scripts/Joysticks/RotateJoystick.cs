using System;
using Game.Scripts.Infrastructure;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JoystickPack.Scripts.Joysticks
{
  public class RotateJoystick : MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler, ICanvasRaycastFilter
  {
    [SerializeField] private float sensitivity = 1;
    [SerializeField] private LayerMask clickThroughMask = ~0;
    public Vector2 delta;
    private Vector2 _startPoint;
    private Camera _camera;

    private void Awake()
    {
      _camera = Camera.main;
    }

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
  }
}