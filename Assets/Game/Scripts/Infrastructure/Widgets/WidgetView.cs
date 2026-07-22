using System;
using System.Collections.Generic;
using Game.Scripts.Infrastructure.Extensions;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Widgets
{
  public abstract class WidgetView<T> : MonoBehaviour where T : WidgetModel
  {
    protected List<IDisposable> Disposables = new();
    
    public void Init(T model)
    {
      Disposables.ForEach(x => x.Dispose());
      Disposables.Clear();
      SetModel(model);
      gameObject.DisposeModel(model);
    }

    protected abstract void SetModel(T model);
  }
}