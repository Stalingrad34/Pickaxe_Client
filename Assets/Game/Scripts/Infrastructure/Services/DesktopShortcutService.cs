using System;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using UniRx;
using YG;

namespace Game.Scripts.Infrastructure.Services
{
  public class DesktopShortcutService : IInitializableService, IStorageProcessor, IDisposable
  {
    public bool IsDirty { get; private set; }
    
    public readonly ReactiveProperty<bool> CanShow = new();
    private readonly PickaxesService _pickaxesService;
    private bool _isSuccess;
    private IDisposable _subscription;

    public DesktopShortcutService(PickaxesService pickaxesService)
    {
      _pickaxesService = pickaxesService;
    }
    
    public void Init()
    {
      _subscription = _pickaxesService.BestPickaxeType.Subscribe(BestPickaxeChanged);
      YG2.onGameLabelSuccess += OnSuccess;
    }
    
    public void Dispose()
    {
      _subscription?.Dispose();
      YG2.onGameLabelSuccess -= OnSuccess;
    }
    
    public void ShowDialog()
    {
      YG2.GameLabelShowDialog();
    }

    private void OnSuccess()
    {
      _isSuccess = true;
      _pickaxesService.AddPickaxe(PickaxeType.Iron, 1);
      IsDirty = true;
    }

    private void BestPickaxeChanged(PickaxeType pickaxeType)
    {
      if (pickaxeType >= PickaxeType.Copper)
        CanShow.Value = YG2.gameLabelCanShow && !_isSuccess;
    }

    public void Save(SaveData data)
    {
      data.Player.DesktopShortcutSuccess = _isSuccess;
      IsDirty = false;
    }

    public void Load(SaveData data)
    {
      
      _isSuccess = data.Player.DesktopShortcutSuccess;
    }
  }
}