using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Cysharp.Threading.Tasks;

namespace YG
{
    [DefaultExecutionOrder(-1)]
    public class ImageLoadYG : MonoBehaviour
    {
        public bool startLoad;
#if PLUGIN_YG_2
        [NestedYG("startLoad")]
#endif
        public string urlImage;

        public RawImage rawImage;
        public Image spriteImage;
        public GameObject loadAnimObj;
        [SerializeField] bool log;

        public Action onTextureLoad;

        private struct LoadTextures { public string link; public Texture2D texture; }
        private static List<LoadTextures> saveTextures = new List<LoadTextures>();

        private void Awake()
        {
            if (rawImage)
                rawImage.enabled = false;
            if (spriteImage)
                spriteImage.enabled = false;

            if (startLoad)
                Load();
            else if (loadAnimObj)
                loadAnimObj.SetActive(false);
        }

        public void Load(string url)
        {
            if (!IsValidUrl(url))
                return;

            Texture2D existingTexture = ExistingTexture(url);
            if (existingTexture)
                SetTexture(existingTexture);
            else
                LoadTexture(url).Forget();
        }
        public void Load() => Load(urlImage);

        private Texture2D ExistingTexture(string url)
        {
            List<LoadTextures> images = saveTextures;

            for (int i = 0; i < images.Count; i++)
            {
                if (url == images[i].link)
                    return images[i].texture;
            }

            return null;
        }

        private bool IsValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url) || url == "null")
                return false;
            
            if (url.StartsWith("//"))
                url = "https:" + url;

            return Uri.TryCreate(url, UriKind.Absolute, out Uri uri) &&
                   (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        public void ClearTexture()
        {
            if (rawImage)
            {
                rawImage.texture = null;
                rawImage.enabled = false;
            }

            if (spriteImage)
            {
                spriteImage.sprite = null;
                spriteImage.enabled = false;
            }

            if (loadAnimObj)
                loadAnimObj.SetActive(false);
        }

        async UniTaskVoid LoadTexture(string url)
        {
            if (loadAnimObj)
                loadAnimObj.SetActive(true);

            using UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);
            try
            {
                await webRequest.SendWebRequest();
            }
            catch (Exception e)
            {
                if (log)
                    Debug.LogError("ImageLoadYG Error: " + e.Message);

                if (loadAnimObj)
                    loadAnimObj.SetActive(false);

                return;
            }

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                if (log)
                    Debug.LogError("ImageLoadYG Error: " + webRequest.error);

                if (loadAnimObj)
                    loadAnimObj.SetActive(false);
            }
            else
            {
                DownloadHandlerTexture handlerTexture = webRequest.downloadHandler as DownloadHandlerTexture;

                if (handlerTexture.isDone)
                {
                    Texture2D existingTexture = ExistingTexture(url);
                    if (existingTexture)
                    {
                        SetTexture(existingTexture);
                    }
                    else
                    {
                        SetTexture(handlerTexture.texture);
                        saveTextures.Add(new LoadTextures
                        {
                            link = url,
                            texture = handlerTexture.texture
                        });
                    }
                }
            }
        }

        public void SetTexture(Texture2D texture)
        {
            if (rawImage)
            {
                rawImage.texture = texture;
                rawImage.enabled = true;
            }

            if (spriteImage)
            {
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                spriteImage.sprite = Sprite.Create(texture, rect, Vector2.zero);
                spriteImage.enabled = true;
            }

            if (loadAnimObj)
            {
                loadAnimObj.SetActive(false);
                onTextureLoad?.Invoke();
            }
        }
    }
}
