using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Gameplay.Pickaxe
{
  public class PickaxesFloorView : MonoBehaviour
  {
    [SerializeField] private List<PickaxeRootView> pickaxeRoots;

    public bool IsFull()
    {
      foreach (var pickaxeRoot in pickaxeRoots)
      {
        if (pickaxeRoot.IsEmpty())
          return false;
      }
      
      return true;
    }

    public void AddView(PickaxeView view)
    {
      gameObject.SetActive(true);
      
      var availableRoot = pickaxeRoots.FirstOrDefault(r => r.IsEmpty());
      availableRoot?.AddView(view);
    }

    public List<PickaxeView> ClearViews()
    {
      var result = new List<PickaxeView>();
      foreach (var root in pickaxeRoots)
      {
        if (root.IsEmpty())
          continue;
        
        result.Add(root.RemoveView());
      }
      
      gameObject.SetActive(false);
      
      return result;
    }

    public void Punch()
    {
      foreach (var root in pickaxeRoots)
      {
        if (root.IsEmpty())
          continue;

        root.Punch().Forget();
      }
    }
  }
}