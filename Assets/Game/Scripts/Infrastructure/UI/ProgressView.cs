using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Infrastructure.UI
{
  public class ProgressView : MonoBehaviour
  {
    [SerializeField] private Slider slider;
    [SerializeField] private float tweenSpeed = 1;
    [SerializeField] private bool decreaseTween;

    private Tween _tween;
    private float? _lastValue;

    public void SetValue(int value, int max)
    {
      var progress = value / (float)max;
      if (_lastValue.HasValue && (progress > _lastValue || decreaseTween))
      {
        StartProgressTween(progress);
      }
      else
      {
        SetProgressImmediately(progress);
        _lastValue = progress;
      }
    }

    private void StartProgressTween(float value)
    {
      _tween.Complete();
      
      var duration = Math.Abs(_lastValue.Value - value) * 10 / Math.Max(tweenSpeed, 1);
      _tween = Tween
        .Custom(_lastValue.Value, value, duration, SetProgressImmediately, Ease.Linear)
        .OnComplete(() => _lastValue = value);
    }

    private void SetProgressImmediately(float value)
    {
      slider.value = value;
    }

    private void OnDestroy()
    {
      _tween.Stop();
    }
  }
}
