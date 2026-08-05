using System.Collections.Generic;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Services;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.Environment.Rating
{
  public class RatingView : MonoBehaviour
  {
    [SerializeField] private RectTransform playersRoot;
    [SerializeField] private PlayerRatingView playerView;
    
    private List<PlayerRatingView> _players = new();
    
    private void Start()
    {
      ServiceProvider.Get<RatingService>().Players.SubscribeAdd(PlayersAdded).AddTo(gameObject);
    }

    private void PlayersAdded(PlayerRatingModel player, int index)
    {
      PlayerRatingView view;
      if (index >= _players.Count)
      {
        view = Instantiate(playerView, playersRoot);
        _players.Add(view);
        view.gameObject.SetActive(true);
      }
      else
        view = _players[index];
      
      view.Init(player);
    }
  }
}