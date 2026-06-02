using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Multiplayer;
using Newtonsoft.Json;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services
{
  public class RatingService : IService
  {
    public class Response
    {
      public bool success;
      public string message;
      public TopPlayer[] top10;
    }
    public class TopPlayer
    {
      public int rank;
      public string name;
      public int slaps;
    }
    
    public readonly ReactiveCollection<TopPlayer> TopPlayers = new ();
    private readonly string _ratingUri;
    private CancellationToken _token;

    public RatingService(ConnectConfig connectConfig)
    {
      _ratingUri = connectConfig.RatingUrl;
    }

    public async UniTaskVoid StartFetchRating()
    {
      var data = new Dictionary<string, string>
      {
        ["user_id"] = PlayerInfo.GetUserId()
      }; 
      
      var result = await HttpService.Post(_ratingUri, data);
      Response response;
      try
      {
        response = JsonConvert.DeserializeObject<Response>(result);
      }
      catch (Exception e)
      {
        Debug.LogError(e);
        return;
      }

      if (!response.success)
      {
        Debug.LogError(response.message);
        return;
      }
      
      Debug.Log(response.message);
      TopPlayers.AddRange(response.top10);
    }

    public void StopFetchRating()
    {
      
    }
  }
}