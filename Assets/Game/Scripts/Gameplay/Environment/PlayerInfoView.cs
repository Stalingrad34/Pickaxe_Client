using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Services;
using TMPro;
using UniRx;
using UnityEngine;
using YG;

namespace Game.Scripts.Gameplay.Environment
{
  public class PlayerInfoView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private ImageLoadYG playerAvatar;
    [SerializeField] private GameObject authRoot;
    [SerializeField] private GameObject notAuthRoot;
    
    private void Start()
    {
      var service = ServiceProvider.Get<PlayerService>();
      service.PlayerName.SubscribeToTMP(playerName).AddTo(gameObject);
      service.PlayerAvatar.Subscribe(AvatarChanged).AddTo(gameObject);
      service.IsAuthorized.Subscribe(IsAuthorizedChanged).AddTo(gameObject);
    }

    private void AvatarChanged(string avatar)
    {
      playerAvatar.Load(avatar);
    }

    private void IsAuthorizedChanged(bool isAuthorized)
    {
      authRoot.SetActive(isAuthorized);
      notAuthRoot.SetActive(!isAuthorized);
    }
  }
}