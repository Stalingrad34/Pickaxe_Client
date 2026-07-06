using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Services;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.Environment
{
  public class ProcessOreMonitorView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI processingOreAmountText;

    private void Start()
    {
      var economy = ServiceProvider.Get<EconomyService>();
      economy.ProcessingOre.SubscribeOre(processingOreAmountText).AddTo(gameObject);
    }
  }
}