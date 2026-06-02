using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Widgets
{
  public class ImageUrlWidget : MonoBehaviour
  {
    [SerializeField] private Image image;
    [SerializeField] private RectTransform loadingImage;
    
    private string _url;

    public async UniTaskVoid SetImageUrl(string url)
    {
      if (_url == url || string.IsNullOrEmpty(url)) 
        return;
      
      _url = url;
      image.enabled = false;
      loadingImage.gameObject.SetActive(true);

      try
      {
        var sprite = await SpriteLoader.LoadSprite(url, destroyCancellationToken);
        if (sprite != null && !destroyCancellationToken.IsCancellationRequested)
          image.sprite = sprite;
        else
          image.sprite = AtlasManager.GetSprite("player");
      }
      catch (Exception e)
      {
        Debug.LogError(e);
        image.sprite = AtlasManager.GetSprite("player");
      }
      
      image.enabled = true;
      loadingImage.gameObject.SetActive(false);
    }
  }
}