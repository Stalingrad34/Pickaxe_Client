using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Scripts.Infrastructure
{
  public class SpriteLoader
  {
    private static readonly Dictionary<string, Sprite> Sprites = new ();
    
    public static async UniTask<Sprite> LoadSprite(string path, CancellationToken token)
    {
      if (Sprites.TryGetValue(path, out var result))
        return result;
    
      using var uwr = UnityWebRequestTexture.GetTexture(path);
      try
      {
        await uwr.SendWebRequest().ToUniTask(cancellationToken:token);
      }
      catch (Exception e)
      {
        Debug.LogError(e);
        return null;
      }
      
      if (token.IsCancellationRequested)
        return null;
      
      if (Sprites.TryGetValue(path, out result))
        return result;

      if (uwr.result == UnityWebRequest.Result.Success)
      {
        var texture = DownloadHandlerTexture.GetContent(uwr);
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        Sprites.TryAdd(path, sprite);
        return sprite;
      }
      
      return null;
    }
  }
}