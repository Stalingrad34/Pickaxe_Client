using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Tutorial
{
  public class TutorialTargetsContainer
  {
    private readonly Dictionary<Type, MonoBehaviour> _targets = new();

    public void Add<T>(T target) where T : MonoBehaviour
    {
      _targets.Add(target.GetType(), target);
    }
    
    public bool Has<T>() where T : MonoBehaviour
    {
      return _targets.ContainsKey(typeof(T));
    }

    public T Get<T>() where T : MonoBehaviour
    {
      if (_targets.TryGetValue(typeof(T), out var target))
      {
        return target as T;
      }
      
      return null;
    }
  }
}