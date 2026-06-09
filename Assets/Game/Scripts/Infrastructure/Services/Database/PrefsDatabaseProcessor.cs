using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay.Pickaxe;
using Newtonsoft.Json;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Database
{
  public class PrefsDatabaseProcessor : IDatabaseProcessor
  {
    private string NAME_KEY = "name";
    private string MONEY_KEY = "money";
    private string ORE_KEY = "ore";
    private string PROCESSING_MONEY_KEY = "processing_money";
    private string PROCESSING_ORE_KEY = "processing_ore";
    private string PICKAXES_KEY = "pickaxes";
    private string PICKAXES_NOMINAL_KEY = "pickaxes_nominal";
    
    public async UniTask Load(DatabaseService database)
    {
      database.PlayerName.Value = PlayerPrefs.GetString(NAME_KEY, GetRandomName());
      database.Money.Value = ulong.TryParse(PlayerPrefs.GetString(MONEY_KEY, "0"), out var m) ? m : 0;
      database.Ore.Value = ulong.TryParse(PlayerPrefs.GetString(ORE_KEY, "0"), out var o) ? o : 0;
      database.ProcessingMoney.Value = ulong.TryParse(PlayerPrefs.GetString(PROCESSING_MONEY_KEY, "0"), out var pm) ? pm : 0;
      database.ProcessingOre.Value = ulong.TryParse(PlayerPrefs.GetString(PROCESSING_ORE_KEY, "0"), out var po) ? po : 0;
      database.PickaxesNominal.Value = ulong.TryParse(PlayerPrefs.GetString(PICKAXES_NOMINAL_KEY, "0"), out var pn) ? pn : 0;

      var pickaxesPrefs = PlayerPrefs.GetString(PICKAXES_KEY, string.Empty);
      if (!string.IsNullOrEmpty(pickaxesPrefs))
      {
        var json = JsonConvert.DeserializeObject<List<PickaxeData>>(pickaxesPrefs);
        database.Pickaxes.AddRange(json);
      }
      
      await UniTask.Yield();
    }

    public async UniTask Save(DatabaseService database)
    {
      PlayerPrefs.SetString(NAME_KEY, database.PlayerName.Value);
      PlayerPrefs.SetString(MONEY_KEY, database.Money.ToString());
      PlayerPrefs.SetString(ORE_KEY, database.Ore.ToString());
      PlayerPrefs.SetString(PROCESSING_MONEY_KEY, database.ProcessingMoney.ToString());
      PlayerPrefs.SetString(PROCESSING_ORE_KEY, database.ProcessingOre.ToString());
      PlayerPrefs.SetString(PICKAXES_NOMINAL_KEY, database.PickaxesNominal.ToString());
      
      var json = JsonConvert.SerializeObject(database.Pickaxes);
      PlayerPrefs.SetString(PICKAXES_KEY, json);
      
      await UniTask.Yield();
    }
    
    private string GetRandomName()
    {
      return $"Player_{Random.Range(1000000, 9999999)}";
    }
  }
}