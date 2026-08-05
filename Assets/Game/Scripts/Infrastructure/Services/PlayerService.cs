using System;
using UniRx;
using YG;

namespace Game.Scripts.Infrastructure.Services
{
  public class PlayerService : IInitializableService, IDisposable
  {
    public readonly ReactiveProperty<string> PlayerName = new();
    public readonly ReactiveProperty<string> PlayerAvatar = new();
    public readonly ReactiveProperty<bool> IsAuthorized = new();
    
    public void Init()
    {
      RefreshPlayerData();
      YG2.onGetSDKData += OnGetSDK;
    }

    public void OpenAuthDialogue()
    {
      YG2.OpenAuthDialog();
    }

    private void RefreshPlayerData()
    {
      PlayerName.Value = YG2.player.name;
      PlayerAvatar.Value = YG2.player.photo;
      IsAuthorized.Value = YG2.player.auth;
    }

    private void OnGetSDK()
    {
      RefreshPlayerData();
    }

    public void Dispose()
    {
      YG2.onGetSDKData -= OnGetSDK;
    }
  }
}