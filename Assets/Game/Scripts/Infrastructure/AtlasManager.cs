using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Game.Scripts.Infrastructure
{
    public class AtlasManager : MonoBehaviour
    {
        private static AtlasManager _instance;
        
        [SerializeField] private List<SpriteAtlas> atlases;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _instance = this;
        }

        public static Sprite GetSprite(string name)
        {
            foreach (var atlas in _instance.atlases)
            {
                var sprite = atlas.GetSprite(name);
                if (sprite != null)
                {
                    sprite.name = sprite.name.Replace("(Clone)", "");
                    return sprite;
                }
            }

            return null;
        }
    }
}