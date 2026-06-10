using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using Game.Scripts.Multiplayer;
using Newtonsoft.Json;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Database
{
  public class MySqlStorageRepository : IStorageRepository
  {
    public class Response
    {
      public bool success;
      public string message;
      public PlayerStateDto data;
    }
    
    public class PlayerStateDto
    {
      [JsonProperty("n")] public string Name;
      [JsonProperty("w")] public string Weapon;
      [JsonProperty("f")] public string Face;
      [JsonProperty("b")] public string Body;
      [JsonProperty("s")] public int Slaps;
      [JsonProperty("i")] public List<string> Inventory = new();
    }
    
    private readonly string _getStateURI;
    private readonly string _setStateURI;
    
    public MySqlStorageRepository(ConnectConfig connectConfig)
    {
      _getStateURI = connectConfig.GetStateUrl;
      _setStateURI = connectConfig.SetStateUrl;
    }
    
    public async UniTask<SaveData> Load()
    {
      var pars = new Dictionary<string, string>
      {
        ["user_id"] = PlayerInfo.GetUserId(),
        ["device_id"] = PlayerInfo.GetDeviceId(),
      };

      Response response;
      var result = await HttpService.Post(_getStateURI, pars);
      try
      {
        response = JsonConvert.DeserializeObject<Response>(result);
      }
      catch (Exception e)
      {
        Debug.LogError(e);
        return new SaveData();
      }

      if (!response.success)
      {
        Debug.LogError(response.message);
        return new SaveData();
      }
      
      Debug.Log(response.message);
      return new SaveData();
      
      /*database.PlayerName.Value = response.data.Name;
      database.Weapon.Value = response.data.Weapon;
      database.Face.Value = response.data.Face;
      database.Body.Value = response.data.Body;
      database.Slaps.Value = response.data.Slaps;
      database.Inventory.AddRange(response.data.Inventory);*/
    }

    public async UniTask Save(SaveData database)
    {
      var state = new PlayerStateDto()
      {
        /*Name = database.PlayerName.Value,
        Weapon = database.Weapon.Value,
        Face = database.Face.Value,
        Body = database.Body.Value,
        Slaps = database.Slaps.Value,
        Inventory = new  List<string>(database.Inventory)*/
      };
      
      var data = new Dictionary<string, string>
      {
        ["user_id"] = PlayerInfo.GetUserId(),
        ["device_id"] = PlayerInfo.GetDeviceId(),
        ["state"] = JsonConvert.SerializeObject(state)
      };
      
      var result = await HttpService.Post(_setStateURI, data);
      
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
    }
  }
}