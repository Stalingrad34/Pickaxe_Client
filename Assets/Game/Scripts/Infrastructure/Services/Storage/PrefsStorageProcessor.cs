using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services.Storage.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Storage
{
  public class PrefsStorageProcessor : IStorageRepository
  {
    private string PLAYER_DATA_KEY = "player_data";
    private string ECONOMY_DATA_KEY = "economy_data";
    private string PICKAXES_DATA_KEY = "pickaxes_data";
    
    public async UniTask<SaveData> Load()
    {
      var playerData = PlayerPrefs.GetString(PLAYER_DATA_KEY);
      var economyData = PlayerPrefs.GetString(ECONOMY_DATA_KEY);
      var pickaxesData = PlayerPrefs.GetString(PICKAXES_DATA_KEY);
      
      var data = new SaveData
      {
        Player = !string.IsNullOrEmpty(playerData) 
          ? JsonConvert.DeserializeObject<PlayerStorageData>(playerData) 
          : new PlayerStorageData(),
        
        Economy = !string.IsNullOrEmpty(economyData) 
          ? JsonConvert.DeserializeObject<EconomyStorageData>(economyData) 
          : new EconomyStorageData(),
        
        Pickaxes = !string.IsNullOrEmpty(pickaxesData) 
          ? JsonConvert.DeserializeObject<PickaxesStorageData>(pickaxesData) 
          : new PickaxesStorageData(),
      };

      await UniTask.Yield();
      return data;
    }

    public async UniTask Save(SaveData database)
    {
      PlayerPrefs.SetString(PLAYER_DATA_KEY, JsonConvert.SerializeObject(database.Player));
      PlayerPrefs.SetString(ECONOMY_DATA_KEY, JsonConvert.SerializeObject(database.Economy));
      PlayerPrefs.SetString(PICKAXES_DATA_KEY, JsonConvert.SerializeObject(database.Pickaxes));
      
      await UniTask.Yield();
    }
    
    private string GetRandomName()
    {
      return $"Player_{Random.Range(1000000, 9999999)}";
    }
  }
}