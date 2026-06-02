using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services.Database
{
  public class PrefsDatabaseProcessor : IDatabaseProcessor
  {
    private string NAME_KEY = "name";
    private string WEAPON_KEY = "weapon";
    private string FACE_KEY = "face";
    private string BODY_KEY = "body";
    private string SLAPS_KEY = "slaps";
    private string PURCHASES_KEY = "purchases";
    
    public async UniTask Load(DatabaseService database)
    {
      database.PlayerName.Value = PlayerPrefs.GetString(NAME_KEY, GetRandomName());
      database.Weapon.Value = PlayerPrefs.GetString(WEAPON_KEY, "Newspaper");
      database.Face.Value = PlayerPrefs.GetString(FACE_KEY, "Face1");
      database.Body.Value = PlayerPrefs.GetString(BODY_KEY, "Body1");
      database.Slaps.Value = PlayerPrefs.GetInt(SLAPS_KEY, 0);
      
      var purchases = PlayerPrefs.GetString(PURCHASES_KEY, "").Split(',');
      if (purchases.Any())
        database.Inventory.AddRange(purchases);
      
      await UniTask.Yield();
    }

    public async UniTask Save(DatabaseService database)
    {
      PlayerPrefs.SetString(NAME_KEY, database.PlayerName.Value);
      PlayerPrefs.SetString(WEAPON_KEY, database.Weapon.Value);
      PlayerPrefs.SetString(FACE_KEY, database.Face.Value);
      PlayerPrefs.SetString(BODY_KEY, database.Body.Value);
      PlayerPrefs.SetInt(SLAPS_KEY, database.Slaps.Value);
      
      var purchases = string.Empty;
      var isFirst = true;
      foreach (var purchase in database.Inventory)
      {
        if (!isFirst)
          purchases += ",";
        
        purchases += purchase;
        isFirst = false;
      }
      
      PlayerPrefs.SetString(PURCHASES_KEY, purchases);
      
      await UniTask.Yield();
    }
    
    private string GetRandomName()
    {
      return $"Player_{Random.Range(1000000, 9999999)}";
    }
  }
}