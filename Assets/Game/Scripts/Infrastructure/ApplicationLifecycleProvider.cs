using System;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Infrastructure
{
  public class ApplicationLifecycleProvider : MonoBehaviour
  {
    public static event Action SecondTime;
    public static event Action ApplicationQuit;
    
    private IDisposable _timer;
    
    private void Awake()
    {
      DontDestroyOnLoad(this);
    }

    private void Start()
    {
      //_timer = Observable.Timer(TimeSpan.FromSeconds(1)).Repeat().Subscribe(_ => SecondTime?.Invoke());
    }

    private void OnApplicationQuit()
    {
      ApplicationQuit?.Invoke();
    }
  }
}