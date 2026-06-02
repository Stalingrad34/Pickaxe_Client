using Game.Scripts.Infrastructure.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts.Loggers
{
    public class LoggerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button close;
        [SerializeField] private Button clear;
        
        private LoggerModel _model;

        public void Init(LoggerModel model)
        {
            _model = model;
            _model.ConsoleText.SubscribeToTMP(text).AddTo(gameObject);
            _model.IsShowed.Subscribe(gameObject.SetActive).AddTo(gameObject);
            close.OnClick(_model.Close).AddTo(gameObject);
            clear.OnClick(_model.ClearLogs).AddTo(gameObject);
        }
    }
}