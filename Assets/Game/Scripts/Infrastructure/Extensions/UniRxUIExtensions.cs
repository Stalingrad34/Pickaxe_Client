using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Game.Scripts.Gameplay;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.UI;
using Game.Scripts.Widgets;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Game.Scripts.Infrastructure.Extensions
{
    public static class UniRxUIExtensions
    {
        public static IDisposable SubscribeMoney(this IObservable<ulong> source, TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = $"${MoneyFormatter.Format((long)x)}");
        }
        
        public static IDisposable SubscribeToTMP(this IObservable<string> source, TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x);
        }
        
        public static IDisposable SubscribeToTMP(this IObservable<int> source, TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x.ToString());
        }
        
        public static IDisposable SubscribeToSlider(this IObservable<float> source, Slider text)
        {
            return source.SubscribeWithState(text, (x, t) => t.value = x);
        }
        
        public static IDisposable SubscribeCustomText(this IObservable<TextData> data, CustomText text)
        {
            return data.SubscribeWithState(text, (x, t) => t.SetText(x));
        }
        
        public static IDisposable SubscribeCustomText(this IObservable<int> data, CustomText text, string format)
        {
            return data.SubscribeWithState(text, (x, t) =>
            {
                var textData = new TextData(format, x.ToString());
                t.SetText(textData);
            });
        }
        
        public static IDisposable SubscribeToButtonText(this IObservable<TextData> source, CustomButton button)
        {
            return SubscribeCustomText(source, button.text);
        }
        
        public static IDisposable SubscribeToAtlasImage(this IObservable<string> source, Image text)
        {
            return source.SubscribeWithState(text, (x, t) =>
            {
                if (string.IsNullOrEmpty(x))
                {
                    t.gameObject.SetActive(false);
                    return;
                }
                
                t.gameObject.SetActive(true);
                t.sprite = AtlasManager.GetSprite(x);
            });
        }
        
        public static IDisposable SubscribeToImageSprite(this IObservable<Sprite> source, Image image)
        {
            return source.SubscribeWithState(image, (c, t) => t.sprite = c);
        }
        
        public static IDisposable SubscribeColor(this IObservable<Color> source, Image image)
        {
            return source.SubscribeWithState(image, (c, t) => t.color = c);
        }
        
        public static IDisposable SubscribeToImageUrl(this IObservable<string> source, ImageUrlWidget widget)
        {
            return source.SubscribeWithState(widget, (c, t) => widget.SetImageUrl(c).Forget());
        }

        public static IDisposable SubscribeToBool(this IObservable<bool> source, Action<bool> method)
        {
            return source.Subscribe(method);
        }

        public static IDisposable SubscribeToTMPColor(this IObservable<Color> source, TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (c, t) => t.color = c);
        }
        
        public static IDisposable SubscribeToTMPColor(this IObservable<string> source, TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (c, t) =>
            {
                if (!ColorUtility.TryParseHtmlString(c, out var color))
                    color = Color.white;
                t.color = color;
            });
        }
        
        public static IDisposable SubscribeToTMPOutline(this IObservable<string> source, TextMeshProUGUI text)
        {
            return source.SubscribeWithState(text, (c, t) =>
            {
                if (!ColorUtility.TryParseHtmlString(c, out var color))
                    color = Color.white;
                t.fontMaterial.SetColor("_OutlineColor", color);
            });
        }

        public static IDisposable SubscribeToImageColor(this IObservable<Color> source, Image image)
        {
            return source.SubscribeWithState(image, (c, t) => t.color = c);
        }
        
        public static IDisposable SubscribeBtnInteractable(this IObservable<bool> source, Button progressBar)
        {
            return source.SubscribeWithState(progressBar, (c, t) => t.interactable = c);
        }
        
        public static IDisposable SubscribeAlpha(this IObservable<float> source, CanvasGroup canvasGroup)
        {
            return source.SubscribeWithState(canvasGroup, (c, t) => t.alpha = c);
        }
        
        public static IDisposable OnClick(this Button button, Action method)
        {
            return button.onClick.AsObservable().Subscribe(_ => method?.Invoke());
        }
        
        public static IDisposable OnClick<T>(this Button button, Action<T> method, T value)
        {
            return button.onClick.AsObservable().Subscribe(_ => method?.Invoke(value));
        }

        public static IDisposable OnClick(this CustomButton button, Action method)
        {
            return button.onClick.AsObservable().Subscribe(_ => method?.Invoke());
        }

        public static IDisposable OnClick(this Button button, ReactiveCommand method)
        {
            return button.onClick.AsObservable().Subscribe(_ => method?.Execute());
        }
        
        public static IDisposable OnValueChanged(this Slider slider, Action<float> callback)
        {
            return slider.onValueChanged.AsObservable().Subscribe(value => callback?.Invoke(value));
        }
        
        public static IDisposable SubscribeCommand(this ReactiveCommand command, Action method)
        {
            return command.Subscribe(_ => method?.Invoke());
        }

        public static IDisposable SubscribeAdd<T>(this ReactiveCollection<T> collection, Action<T, int> method)
        {
            foreach (var item in collection)
            {
                method?.Invoke(item, collection.IndexOf(item));
            }
            return collection.ObserveAdd().Subscribe(addEvent => method?.Invoke(addEvent.Value, addEvent.Index));
        }
        
        public static IDisposable SubscribeAdd<T, U>(this ReactiveDictionary<T, U> collection, Action<T, U> method)
        {
            foreach (var kvp in collection)
            {
                method?.Invoke(kvp.Key, kvp.Value);
            }
            return collection.ObserveAdd().Subscribe(addEvent => method?.Invoke(addEvent.Key, addEvent.Value));
        }
        
        public static IDisposable SubscribeReplace<T, U>(this ReactiveDictionary<T, U> collection, Action<T, U, U> method)
        {
            foreach (var kvp in collection)
            {
                method?.Invoke(kvp.Key, kvp.Value, kvp.Value);
            }
            
            return collection.ObserveReplace().Subscribe(addEvent => method?.Invoke(addEvent.Key, addEvent.OldValue, addEvent.NewValue));
        }
        
        public static IDisposable SubscribeRemove<T, U>(this ReactiveDictionary<T, U> collection, Action<T, U> method)
        {
            return collection.ObserveRemove().Subscribe(addEvent => method?.Invoke(addEvent.Key, addEvent.Value));
        }
        
        public static IDisposable SubscribeRemove<T>(this ReactiveCollection<T> collection, Action<T, int> method)
        {
            return collection.ObserveRemove().Subscribe(addEvent => method?.Invoke(addEvent.Value, addEvent.Index));
        }

        public static IDisposable SubscribeCustomStyle(this IObservable<CustomButtonSettings.Style> source, CustomButton button)
        {
            return source.SubscribeWithState(button, (x, t) =>
            {
                var styleSettings = CustomButtonSettings.GetButtonStyle(x);
                t.SetupStyle(styleSettings);
            });
        }
        
        public static IDisposable SubscribeImageFill(this IObservable<float> source, Image image)
        {
            return source.SubscribeWithState(image, (x, t) =>
            {
                t.fillAmount = x;
            });
        }

        public static void DisposeModel(this GameObject go, DisposableModel model)
        {
            go.OnDestroyAsync().ToObservable().Subscribe(_ => model.Dispose());
        }
    }
}