using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
      _view.transform.localRotation = Quaternion.identity;
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

    public async UniTaskVoid Punch()
    {
      if (_isEmpty)
        return;
      
      var token = destroyCancellationToken;
      
      await _view.PlayPunchAnimation(token);
      if (token.IsCancellationRequested)
        return;
    
      _view.SpawnOre();
    }

    public bool IsEmpty()
    {
      return _isEmpty;
    }
  }
}