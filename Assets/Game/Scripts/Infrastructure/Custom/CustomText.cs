using System.Collections.Generic;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Services;
using TMPro;
using UniRx;

namespace Game.Scripts.Infrastructure.Custom
{
    public class CustomText : TextMeshProUGUI
    {
        private TextData _textData;
        private string[] _params;
        private bool _isSubscribed;

        private LocalizationService LocalizationService => ServiceProvider.Get<LocalizationService>();

        public void SetText(TextData textData)
        {
            if (textData == null)
                return;
            
            _textData = textData;
            text = textData.Key;
            
            if (!_isSubscribed)
            {
                _isSubscribed = true;
                LocalizationService.LanguageChanged.SubscribeCommand(OnLanguageChanged).AddTo(gameObject);
            }
            
            LocalizeText();
        }

        private void OnLanguageChanged()
        {
            LocalizeText();
        }

        private void LocalizeText()
        {
            if (!string.IsNullOrEmpty(_textData.Key))
            {
                text = LocalizationService.GetLocalizedText(_textData.Key);
            }
            else if (!string.IsNullOrEmpty(_textData.Format))
            {
                var format = LocalizationService.GetLocalizedText(_textData.Format);

                var values = new List<string>(_textData.Values.Length);
                foreach (var value in _textData.Values)
                {
                    var localizeValues = LocalizationService.GetLocalizedText(value);
                    values.Add(localizeValues);
                }

                text = string.Format(format, values.ToArray());
            }
            else
                text = string.Empty;
        }
    }
}