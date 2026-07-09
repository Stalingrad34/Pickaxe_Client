using Game.Scripts.Infrastructure.Services;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Gameplay.Environment
{
  public class OreMultiplierView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private TextMeshProUGUI multiplierTimerText;
    [SerializeField] private Image progressBar;

    private void Start()
    {
      var service = ServiceProvider.Get<OreProcessingService>();
      service.ProcessingMultiplier.Subscribe(MultiplierChanged).AddTo(gameObject);
      service.MultiplierTimerSeconds.Subscribe(MultiplierTimerChanged).AddTo(gameObject);
    }

    private void MultiplierChanged(float multiplier)
    {
      var text = multiplier.ToString("0.0");
      multiplierText.text = $"{text}x";
      var value = Mathf.InverseLerp(0.5f, 1.5f, multiplier);
      progressBar.fillAmount = value;
    }

    private void MultiplierTimerChanged(int seconds)
    {
      multiplierTimerText.text = $"Следующее изменение через {seconds} секунд";
    }
  }
}