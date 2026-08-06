using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Services;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.GUI
{
  public class SoundButtonView : MonoBehaviour
  {
    [SerializeField] private Image icon;
    [SerializeField] private Sprite enabledIcon;
    [SerializeField] private Sprite disabledIcon;
    [SerializeField] private CustomButton actionBtn;
    
    private void Start()
    {
      var settings = ServiceProvider.Get<SettingsProvider>();
      settings.SoundDisabled.Subscribe(SoundChanged).AddTo(gameObject);
      actionBtn.OnClick(settings.ChangeSoundDisabled).AddTo(gameObject);
    }

    private void SoundChanged(bool isDisabled)
    {
      icon.sprite = isDisabled ? disabledIcon : enabledIcon;
    }
  }
}