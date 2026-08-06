using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using Game.Scripts.Tutorial;
using Game.Scripts.Tutorial.StartingTutorial;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services
{
  public class TutorialService : IInitializableService, IStorageProcessor, IDisposable
  {
    public bool IsDirty => _currentTutorial?.IsDirty ?? false;
    
    public readonly ReactiveCollection<TutorialType> CompletedTutorials = new();
    
    private readonly List<AbstractTutorial> _tutorials = new();
    private readonly TutorialTargetsContainer _targetsContainer = new();
    private readonly CancellationTokenSource _token = new();
    private AbstractTutorial _currentTutorial;

    public void Init()
    {
      _tutorials.Add(new StartingTutorial());
    }

    public void RegisterTarget<T>(T target) where T: MonoBehaviour
    {
      _targetsContainer.Add(target);
    }
    
    public async UniTaskVoid StartTutorials()
    {
      if (_currentTutorial?.CanStart() ?? false)
        await StartTutorial(_currentTutorial);
      
      while (true)
      {
        foreach (var tutorial in _tutorials)
        {
          if (tutorial.CanStart())
          {
            await StartTutorial(tutorial);
            CompletedTutorials.Add(tutorial.Type);
          }
        }
        
        await UniTask.Yield(cancellationToken:_token.Token).SuppressCancellationThrow();
        if (_token.IsCancellationRequested)
          return;
      }
    }

    public bool IsCompleted(TutorialType type)
    {
      return _tutorials.FirstOrDefault(t => t.Type == type)?.IsCompleted() ?? false;
    }

    private async UniTask StartTutorial(AbstractTutorial tutorial)
    {
      _currentTutorial = tutorial;
      await tutorial.StartTutorial(_targetsContainer, _token.Token);
    }
    
    public void Save(SaveData data)
    {
      _currentTutorial?.Save(data);
    }

    public void Load(SaveData data)
    {
      foreach (var tutorial in _tutorials)
      {
        tutorial.Load(data);
        if (tutorial.IsCompleted())
          CompletedTutorials.Add(tutorial.Type);
      }
      
      var tutorialType = data.Tutorials.CurrentTutorialType;
      _currentTutorial = _tutorials.FirstOrDefault(t => t.Type == tutorialType);
    }

    public void Dispose()
    {
      _token?.Dispose();
    }
  }
}