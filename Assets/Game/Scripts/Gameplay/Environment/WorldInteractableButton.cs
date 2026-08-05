using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Gameplay.Environment
{
  [RequireComponent(typeof(BoxCollider))]
  public abstract class WorldInteractableButton : MonoBehaviour, IPointerClickHandler
  {
    public Texture2D hoverCursor;
    public Vector2 hoverCursorHotspot = new Vector2(26f, 6f);
    private Vector3 _startScale;

    public virtual void Awake()
    {
      _startScale = transform.localScale;
    }

    private void OnDisable()
    {
      ResetCursor();
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
      OnClick();
    }
    
    protected abstract void OnClick();
        
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