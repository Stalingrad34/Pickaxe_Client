using UnityEngine;

namespace Game.Scripts.UI
{
  public abstract class PopupViewBase : MonoBehaviour
  {
    protected CanvasHolder CanvasHolder;
    
    public void SetInputActive(bool isActive)
    {
      CanvasHolder.SetActiveRaycaster(isActive);
    }

    public void Destroy()
    {
      Destroy(CanvasHolder.gameObject);
    }
  }
}
