using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Game.Scripts.Multiplayer
{
  public class HttpService
  {
    public static async UniTask<string> Post(string uri, Dictionary<string, string> data, Action<string> onError = null)
    {
      using var www = UnityWebRequest.Post(uri, data);
      await www.SendWebRequest();
      
      if (www.result == UnityWebRequest.Result.Success) 
        return www.downloadHandler.text;
      
      onError?.Invoke(www.error);
      return string.Empty;
    }
    
    public static async UniTask<string> Get(string uri, Action<string> onError = null)
    {
      using var www = UnityWebRequest.Get(uri);
      await www.SendWebRequest();
      
      if (www.result == UnityWebRequest.Result.Success) 
        return www.downloadHandler.text;
      
      onError?.Invoke(www.error);
      return string.Empty;
    }
  }
}