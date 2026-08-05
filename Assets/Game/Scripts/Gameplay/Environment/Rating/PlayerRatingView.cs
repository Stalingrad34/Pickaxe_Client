using System;
using System.Collections.Generic;
using Game.Scripts.Infrastructure.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using YG;

namespace Game.Scripts.Gameplay.Environment.Rating
{
  public class PlayerRatingView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private ImageLoadYG playerAvatar;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private TextMeshProUGUI score;
    
    private readonly List<IDisposable> _disposables = new();
    
    public void Init(PlayerRatingModel model)
    {
      Dispose();
      
      model.Name.SubscribeToTMP(playerName).AddTo(_disposables);
      model.Avatar.Subscribe(AvatarChanged).AddTo(_disposables);
      model.Rank.SubscribeToTMP(rank).AddTo(_disposables);
      model.Score.SubscribeToTMP(score).AddTo(_disposables);
      model.IsPlayer.Subscribe(IsPlayerChanged).AddTo(_disposables);
    }

    private void AvatarChanged(string avatar)
    {
      playerAvatar.Load(avatar);
    }

    private void IsPlayerChanged(bool isPlayer)
    {
      playerName.color = isPlayer ? Color.green : Color.white;
      rank.color = isPlayer ? Color.green : Color.white;
      score.color = isPlayer ? Color.green : Color.white;
    }

    private void Dispose()
    {
      _disposables.ForEach(x => x.Dispose());
      _disposables.Clear();
    }
  }
}