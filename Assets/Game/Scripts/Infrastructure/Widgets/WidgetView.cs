using Game.Scripts.Infrastructure.Extensions;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Widgets
{
  public abstract class WidgetView<T> : MonoBehaviour where T : WidgetModel
  {
    public void Init(T model)
    {
      SetModel(model);
      gameObject.DisposeModel(model);
    }

    protected abstract void SetModel(T model);
  }
}