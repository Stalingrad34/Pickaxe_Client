using System;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using UniRx;
using YG;

namespace Game.Scripts.Infrastructure.Services
{
  public class ReviewService : IInitializableService, IStorageProcessor, IDisposable
  {
    private readonly PickaxesService _pickaxesService;
    public bool IsDirty { get; private set; }
    public readonly ReactiveProperty<bool> IsAvailable = new();
    private bool _isSuccess;
    private IDisposable _subscription;

    public ReviewService(PickaxesService pickaxesService)
    {
      _pickaxesService = pickaxesService;
    }
    
    public void Init()
    {
      YG2.onReviewSent += OnReviewSent;
      _subscription = _pickaxesService.BestPickaxeType.Subscribe(BestPickaxeChanged);
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
      return YG2.reviewCanShow && !_isSuccess;
    }

    private void BestPickaxeChanged(PickaxeType type)
    {
      IsAvailable.Value = CanShow() && type >= PickaxeType.Iron; 
    }

    private void OnReviewSent(bool isSuccess)
    {
      if (isSuccess)
      {
        IsDirty = true;
        IsAvailable.Value = false;
        _isSuccess = true;
        ServiceProvider.Get<EconomyService>().IncreaseMoney(1000);
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
    }
  }
}