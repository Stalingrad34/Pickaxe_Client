using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Infrastructure;
using UnityEngine;

namespace Game.Scripts.Gameplay.Pickaxe
{
  public class PickaxesHolder : MonoBehaviour
  {
    [SerializeField] private Transform pickaxesPoolRoot;
    [SerializeField] private List<PickaxeRootView> pickaxeRoots;

    private readonly Dictionary<PickaxeType, Queue<PickaxeView>> _views = new();
    
    public void RebuildPickaxes(Dictionary<PickaxeType, int> pickaxes)
    {
      ClearPickaxes();

      var rootIdx = 0;
      var sorted = pickaxes.OrderByDescending(kvp => kvp.Key);
      foreach (var type in sorted)
      {
        for (int i = 0; i < type.Value; i++)
        {
          var view = GetPickaxeView(type.Key);
          pickaxeRoots[rootIdx].AddView(view);
          rootIdx++;
        }
      }
    }
    
    public void PickaxesPunch()
    {
      foreach (var root in pickaxeRoots)
      {
        if (root.IsEmpty())
          continue;
        
        root.Punch();
      }
    }

    private void ClearPickaxes()
    {
      _views.Clear();
      foreach (var root in pickaxeRoots)
      {
        if (root.IsEmpty()) 
          continue;
        
        var view = root.RemoveView();
        view.transform.SetParent(pickaxesPoolRoot);

        if (!_views.ContainsKey(view.GetPickaxeType()))
          _views[view.GetPickaxeType()] = new Queue<PickaxeView>();
          
        _views[view.GetPickaxeType()].Enqueue(view);
      }
    }

    private PickaxeView GetPickaxeView(PickaxeType type)
    {
      if (_views.TryGetValue(type, out var pickaxeViews) && pickaxeViews.Count > 0)
        return pickaxeViews.Dequeue();

      var config = AssetProvider.GetPickaxeData(type);
      var view = Instantiate(config.pickaxeView);
      view.Init(config);

      return view;
    }
  }
}