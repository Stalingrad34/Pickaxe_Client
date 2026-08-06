using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;

namespace Game.Scripts.Tutorial
{
  public abstract class AbstractTutorial : IStorageProcessor
  {
    public bool IsDirty { get; private set; }
    public abstract TutorialType Type { get; }

    private int _currentStep;
    private bool _isCompleted;
    protected readonly List<ITutorialStep> Steps;

    protected AbstractTutorial()
    {
      Steps = GetSteps();
    }

    public bool CanStart()
    {
      if (_isCompleted)
        return false;

      return CanStartInternal();
    }

    public bool IsCompleted()
    {
      return _isCompleted;
    }

    protected abstract bool CanStartInternal();
    protected abstract List<ITutorialStep> GetSteps();

    public async UniTask StartTutorial(TutorialTargetsContainer targetsContainer, CancellationToken token)
    {
      IsDirty = true;
      
      while (_currentStep < Steps.Count)
      {
        var step = Steps[_currentStep];
        await step.Run(targetsContainer, token);
        step.Stop();
        
        if (token.IsCancellationRequested)
          return;
       
        _currentStep++;
        IsDirty = true;
      }
      
      _isCompleted = true;
    }
    
    public void Save(SaveData data)
    {
      var tutorialData = new TutorialData()
      {
        CurrentStep = _currentStep,
        IsCompleted = _isCompleted,
      };

      data.Tutorials.Data[Type] = tutorialData;
      data.Tutorials.CurrentTutorialType = Type;

      IsDirty = false;
    }

    public void Load(SaveData data)
    {
      if (data.Tutorials.Data.TryGetValue(Type, out TutorialData tutorialData))
      {
        _currentStep = tutorialData.CurrentStep;
        _isCompleted = tutorialData.IsCompleted;
      }
    }
  }
}