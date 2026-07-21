using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Widgets;
using PrimeTween;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Scripts.Infrastructure.UI
{
    public class UIManager : MonoBehaviour
    {
        public static Camera GameCamera => _instance._gameCamera; 
        private static UIManager _instance;
        
        private static readonly Vector3 CharacterRawPos = new (1.13f, -1.22f, 4.1f);
        
        [SerializeField] private Canvas guiCanvas;
        [SerializeField] private Canvas loadingScreen;
        [SerializeField] private CanvasHolder canvasHolder;
        [SerializeField] private Transform popupRoot;
        [SerializeField] private Camera uiCamera;
        [SerializeField] private Camera guiCamera;
        [SerializeField] private CanvasGroup blackScreenFade;
        [SerializeField] private NotificationView infoMessageView;
        [SerializeField] private NotificationView warningMessageView;

        private readonly Dictionary<Type, PopupViewBase> _popups = new ();
        private readonly Dictionary<GUIModel, GUIViewBase> _gui = new ();
        private Camera _gameCamera;
        private Sequence _sequence;

        public float duration = 1;
        public float strength = 3;
        public int vibrato = 10;
        public float randomness = 90;

        [ContextMenu("TestShakeCamera")]
        public static void ShakeCamera(float delay)
        {
            Tween.ShakeLocalPosition(
                GameCamera.transform,
                Vector3.one * _instance.strength,
                _instance.duration,
                frequency: _instance.vibrato,
                startDelay: delay);
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _instance = this;
        }

        public static void ShowLoadingScreen(bool isShow)
        {
            _instance.loadingScreen.gameObject.SetActive(isShow);
        }

        public static void SetCameraPosition(Vector3 position)
        {
            GameCamera.transform.position = position;
        }
        
        public static void SetCameraPositionTween(Vector3 position, Action onComplete = null)
        {
            Tween.Position(GameCamera.transform, position, 1, Ease.OutSine)
                .OnComplete(() => onComplete?.Invoke());
        }
        
        public static void SetCameraRotation(Vector3 rotation)
        {
            GameCamera.transform.rotation = Quaternion.Euler(rotation);
        }

        public static TView ShowGUI<TView, TModel>(TModel model) where TView : GUIView<TModel> where TModel : GUIModel
        {
            var resource = AssetProvider.GetGUI<TView, TModel>();
            var view = Instantiate(resource, _instance.guiCanvas.transform);
            view.Init(model);
            _instance._gui[model] = view;

            return view;
        }

        public static bool ShowPopup<TView, TModel>(TModel model)
            where TView : PopupView<TModel> where TModel : PopupModel
        {
            if (_instance._popups.ContainsKey(typeof(TModel)))
                return false;
            
            var res = AssetProvider.GetPopup<TView, TModel>();
            var canvas = GetPopupCanvas();
            var popupView = Instantiate(res, canvas.transform, false);

            popupView.Init(model, canvas);
            _instance._popups[typeof(TModel)] = popupView;

            return true;
        }
        
        public static void HidePopup<T>(T model) where T: PopupModel
        {
            var view = _instance._popups[model.GetType()];
            view.SetInputActive(false);
            DestroyPopup(model);
        }
        
        public static async UniTask ShowBlackScreenFade()
        {
            _instance._sequence.Complete();
            _instance._sequence = Sequence.Create(sequenceEase: Ease.Linear)
                .ChainCallback(() => _instance.blackScreenFade.gameObject.SetActive(true))
                .Chain(Tween.Alpha(_instance.blackScreenFade, 1, 0.5f, Ease.Linear));
            
            await _instance._sequence;
        }
        
        public static async UniTask HideBlackScreenFade()
        {
            _instance._sequence.Complete();
            _instance._sequence = Sequence.Create(sequenceEase: Ease.Linear)
                .Chain(Tween.Alpha(_instance.blackScreenFade, 0, 0.5f, Ease.Linear))
                .ChainCallback(() => _instance.blackScreenFade.gameObject.SetActive(false));
            
            await _instance._sequence;
        }

        public static void ShowInfoMessage(string message)
        {
            _instance.warningMessageView.Hide();
            _instance.infoMessageView.Show(message);
        }
        
        public static void ShowWarnMessage(string message)
        {
            _instance.infoMessageView.Hide();
            _instance.warningMessageView.Show(message);
        }
        
        public static void Clear()
        {
            foreach (var popup in _instance._popups.Values)
            {
                popup.Destroy();
            }

            foreach (var gui in _instance._gui.Values)
            {
                Destroy(gui.gameObject);
            }
            
            _instance._popups.Clear();
            _instance._gui.Clear();
        }
        
        private static CanvasHolder GetPopupCanvas()
        {
            var canvas = Instantiate(_instance.canvasHolder, _instance.popupRoot);
            canvas.name = $"Canvas {_instance._popups.Count}";
            canvas.SetCamera(_instance.uiCamera);

            return canvas;
        }

        private static void DestroyPopup<T>(T model) where T: PopupModel
        {
            var view = _instance._popups[model.GetType()];
            view.Destroy();
            _instance._popups.Remove(model.GetType());
        }

        public static void SetCameraStack(Camera main)
        {
            _instance._gameCamera = main;
            var cameraData = main.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(_instance.guiCamera);
            cameraData.cameraStack.Add(_instance.uiCamera);
        }
    }
}
