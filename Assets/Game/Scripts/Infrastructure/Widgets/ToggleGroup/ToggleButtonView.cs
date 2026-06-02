using System;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Extensions;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Widgets.ToggleGroup
{
  public abstract class ToggleButtonView<T> : MonoBehaviour where T : Enum
  {
    [SerializeField] protected T toggleType;
    [SerializeField] protected CustomButton actionButton;
    
    private ToggleGroupModel<T> _model;

    public void Init(ToggleGroupModel<T> model)
    {
      _model = model;
      SetModel(model);
      actionButton.OnClick(OnClicked).AddTo(gameObject);
    }

    protected abstract void SetModel(ToggleGroupModel<T> model);

    private void OnClicked()
    {
      _model.OnClicked(toggleType);
    }
  }
}