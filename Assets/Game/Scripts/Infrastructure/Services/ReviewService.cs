using System;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using Game.Scripts.Tutorial;
using UniRx;
using YG;

namespace Game.Scripts.Infrastructure.Services
{
  public class ReviewService : IInitializableService, IStorageProcessor, IDisposable
  {
    private readonly TutorialService _tutorialService;
    private readonly PickaxesService _pickaxesService;
    public bool IsDirty { get; private set; }
    public readonly ReactiveProperty<bool> IsAvailable = new();
    private bool _isSuccess;
    private IDisposable _subscription;

    public ReviewService(TutorialService tutorialService, PickaxesService pickaxesService)
    {
      _tutorialService = tutorialService;
      _pickaxesService = pickaxesService;
    }
    
    public void Init()
    {
      YG2.onReviewSent += OnReviewSent;
      _subscription = _tutorialService.CompletedTutorials.SubscribeAdd(OnTutorialCompleted);
    }
    
    public void Dispose()
    {
      YG2.onReviewSent -= OnReviewSent;
      _subscription?.Dispose();
    }

    public void OpenReview()
    {
      YG2.ReviewShow();
    }

    private bool CanShow()
    {
      return YG2.reviewCanShow && !_isSuccess && _tutorialService.IsCompleted(TutorialType.StartingTutorial);
    }

    private void OnTutorialCompleted(TutorialType tutorialType, int _)
    {
      IsAvailable.Value = CanShow(); 
    }

    private void OnReviewSent(bool isSuccess)
    {
      if (isSuccess)
      {
        IsDirty = true;
        IsAvailable.Value = false;
        _isSuccess = true;
      }
    }
    
    public void Save(SaveData data)
    {
      data.Player.ReviewSuccess = _isSuccess;
      IsDirty = false;
    }

    public void Load(SaveData data)
    {
      _isSuccess = data.Player.ReviewSuccess;
      IsAvailable.Value = CanShow();
    }
  }
}