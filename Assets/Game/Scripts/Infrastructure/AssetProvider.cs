using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Gameplay.Units;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.UI;
using Game.Scripts.Multiplayer;
using Game.Scripts.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Scripts.Infrastructure
{
    public static class AssetProvider
    {
        private const string GUI_PATH = "GUI";
        private const string POPUPS_PATH = "Popups";
        private const string SOUNDS_PATH = "Sounds";
        private const string UNITS_PATH = "Units";
        private const string PICKAXES_PATH = "Configs/Pickaxes";
        private static readonly Dictionary<Type, MonoBehaviour> Assets = new();
        
        public static PlayerView GetPlayerView(string prefabPath)
        {
            return GetResource<PlayerView>($"{UNITS_PATH}/{prefabPath}");
        }
        
        public static EnemyView GetEnemyView(string prefabPath)
        {
            return GetResource<EnemyView>($"{UNITS_PATH}/{prefabPath}");
        }
        
        public static PickaxeConfig GetPickaxeData(PickaxeType type)
        {
            return GetResource<PickaxeConfig>($"{PICKAXES_PATH}/Pickaxe_{type}");
        }
        
        public static UnitView GetCharacterView(string prefabPath)
        {
            return GetResource<UnitView>(prefabPath);
        }
        
        public static T GetPopup<T, U>() where T : PopupView<U> where U : PopupModel
        {
            if (Assets.ContainsKey(typeof(T)))
                return Assets[typeof(T)] as T;

            var path = $"{POPUPS_PATH}/{typeof(T).Name}";
            T res = Resources.Load<T>(path);
            if (res == null)
                throw new Exception($"Can't find popup with type {typeof(T).Name} by path {path}");

            Assets.Add(typeof(T), res);

            return res;
        }
        
        public static T GetGUI<T, U>() where T : GUIView<U> where U : GUIModel
        {
            if (Assets.ContainsKey(typeof(T)))
                return Assets[typeof(T)] as T;

            var path = $"{GUI_PATH}/{typeof(T).Name}";
            T res = Resources.Load<T>(path);
            if (res == null)
                throw new Exception($"Can't find GUI with type {typeof(T).Name} by path {path}");

            Assets.Add(typeof(T), res);

            return res;
        }
        
        public static List<AudioClip> GetAllSounds()
        {
            return Resources.LoadAll<AudioClip>($"{SOUNDS_PATH}").ToList();
        }
        
        private static T GetResource<T>(string path) where T : Object
        {
            var resource = Resources.Load<T>(path);
            if (resource == null)
            {
                Debug.LogError($"Resource by path {path} not found");
                return null;
            }
            
            var data = Object.Instantiate(resource);
            data.name = data.name.Replace("_", " ");
            data.name = data.name.Replace("(Clone)", "");

            return data;
        }

        public static CustomButtonStyle GetCustomButtonStyle(CustomButtonSettings.Style style)
        {
            return Resources.Load<CustomButtonStyle>($"Buttons/{style}");
        }

        public static List<PickaxeConfig> GetAllPickaxes()
        {
            var resources = Resources.LoadAll<PickaxeConfig>("Configs/Pickaxes");
            return resources.ToList();
        }
    }
}