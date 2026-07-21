using System;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Infrastructure.UI
{
  public class CounterView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float tweenSpeed = 30;
    [SerializeField] private bool decreaseTween;
    [SerializeField] private string maxFormat = "{0}/{1}";
    [SerializeField] private bool isSeconds;

    private Tween _tween;
    private int? _lastCounter;

    public void SetCounter(int counter)
    {
      SetCounter(counter, null);
    }

    public void SetCounter(int counter, int? max)
    {
      if (_lastCounter.HasValue && (counter > _lastCounter || decreaseTween))
      {
        StartCounterTween(counter, max);
      }
      else
      {
        _tween.Complete();
        SetCounterImmediately(counter, max);
        _lastCounter = counter;
      }
    }

    private void StartCounterTween(int counter, int? max)
    {
      _tween.Complete();
      
      var duration = Math.Abs(_lastCounter.Value - counter) / Math.Max(tweenSpeed, 1);
      _tween = Tween
        .Custom(_lastCounter.Value, counter, duration, value => SetCounterImmediately(Mathf.RoundToInt(value), max), Ease.Linear)
        .OnComplete(() => _lastCounter = counter);
    }

    private void SetCounterImmediately(int counter, int? max)
    {
      var valueText = GetValueText(counter);
      var format = string.IsNullOrEmpty(maxFormat) ? "{0}/{1}" : maxFormat;
      text.text = max.HasValue ? string.Format(format, valueText, max) : valueText;
    }

    private string GetValueText(int counter)
    {
      return isSeconds ? $"{counter / 1000f}" : counter.ToString();
    }

    private void OnDestroy()
    {
      _tween.Stop();
    }
  }
}
