using System;
using System.Collections.Generic;
using Game.Scripts.Gameplay.Environment.Rating;
using Sirenix.Utilities;
using UniRx;
using YG;
using YG.Utils.LB;

namespace Game.Scripts.Infrastructure.Services
{
  public class RatingService : IInitializableService, IDisposable
  {
    public readonly ReactiveCollection<PlayerRatingModel> Players = new();
    private readonly PlayerService _playerService;

    public RatingService(PlayerService playerService)
    {
      _playerService = playerService;
    }
    
    public void Init()
    {
      YG2.onGetLeaderboard += OnGetLeaderBoardHandler;
      YG2.GetLeaderboard("pickaxes", 10, 1);
    }

    public void SetPlayerScore(ulong score)
    {
      if (score >= int.MaxValue)
        score = int.MaxValue;
      
      if (_playerService.IsAuthorized.Value)
        YG2.SetLeaderboard("pickaxes", (int)score);
      
      YG2.GetLeaderboard("pickaxes", 10, 1);
    }

    private void OnGetLeaderBoardHandler(LBData data)
    {
      Players.Clear();
      Players.AddRange(GetPlayerModels(data.players, data.currentPlayer));
    }

    private List<PlayerRatingModel> GetPlayerModels(LBPlayerData[] players, LBCurrentPlayerData currentPlayer)
    {
      var result = new List<PlayerRatingModel>();
      var hasPlayer = false;
      
      for (int i = 0; i < players.Length; i++)
      {
        var player = players[i];
        if (player.rank == currentPlayer?.rank)
          hasPlayer = true;
        
        var model = i == players.Length - 1 && !hasPlayer && _playerService.IsAuthorized.Value && currentPlayer != null
          ? new PlayerRatingModel(currentPlayer, _playerService.PlayerName.Value, _playerService.PlayerAvatar.Value) 
          : new PlayerRatingModel(player, player.rank == currentPlayer?.rank);
        
        result.Add(model);
      }
      
      return result;
    }

    public void Dispose()
    {
      YG2.onGetLeaderboard -= OnGetLeaderBoardHandler;
    }
  }
}