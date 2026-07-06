using Game.Scripts.Gameplay.OreProcessing;
using Game.Scripts.Infrastructure.Services;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.Environment
{
  public class ProcessOreMonitorView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI processingOreAmountText;
    [SerializeField] private TextMeshProUGUI stageOreAmountText;

    private void Start()
    {
      var economy = ServiceProvider.Get<EconomyService>();
      economy.ProcessingOre.Subscribe(ProcessingOreChanged).AddTo(gameObject);
      economy.ProcessingStage.Subscribe(StageChanged).AddTo(gameObject);
    }

    private void ProcessingOreChanged(ulong amount)
    {
      title.text = amount > 0 ? "Обработка руды" : "Нет руды";
      processingOreAmountText.text = MoneyFormatter.Format((long)amount);
    }

    private void StageChanged(int stage)
    {
      var amount = ServiceProvider.Get<OreProcessingService>().GetOrePerSecond(stage);
      stageOreAmountText.text = $"{MoneyFormatter.Format(amount)} за секунду";
    }
  }
}