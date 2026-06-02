using System;
using PrimeTween;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Extensions
{
  public static class TweenExtensions
  {
    public static Tween Punch(this Transform transform, float power, Action callback = null)
    {
      return Tween
        .PunchScale(transform, Vector3.one * power, 0.5f, frequency: 1, easeBetweenShakes: Ease.Linear)
        .OnComplete(callback);
    }
  }
}
