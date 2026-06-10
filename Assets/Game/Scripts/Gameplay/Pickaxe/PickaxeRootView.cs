using UnityEngine;

namespace Game.Scripts.Gameplay.Pickaxe
{
  public class PickaxeRootView : MonoBehaviour
  {
    private bool _isEmpty = true;
    private PickaxeView _view;
    
    public void AddView(PickaxeView view)
    {
      _view = view;
      _view.transform.SetParent(transform);
      _view.transform.localPosition = Vector3.zero;
      _view.gameObject.SetActive(true);
      _isEmpty = false;
    }

    public PickaxeView RemoveView()
    {
      if (_isEmpty)
        return null;
      
      _isEmpty = true;
      _view.gameObject.SetActive(false);
      return _view;
    }

    public bool IsEmpty()
    {
      return _isEmpty;
    }
  }
}