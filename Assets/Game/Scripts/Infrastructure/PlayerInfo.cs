using UnityEngine;
using YG;

namespace Game.Scripts.Infrastructure
{
  public static class PlayerInfo
  {
    private const string ID_KEY = "id";
    
    public static string GetUserId()
    {
      var userId = PlayerPrefs.GetInt(ID_KEY);
      if (userId == 0)
      {
        userId = Random.Range(1000000, 9999999);
        PlayerPrefs.SetInt(ID_KEY, userId);
      }
      
      return userId.ToString();
    }
    
    public static string GetDeviceId()
    {
      if (Platform.IsWebGL())
      {
        return YG2.player.auth ? YG2.player.id : string.Empty;
      }
      
      return string.Empty;
    }
  }
}