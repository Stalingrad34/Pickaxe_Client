using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Services;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.Environment
{
  public class ProcessOreMonitorView : MonoBehaviour
  {
    [SerializeField] private CustomText title;
    [SerializeField] private TextMeshProUGUI processingOreAmountText;
    [SerializeField] private CustomText stageOreAmountText;

    private void Start()
    {
      var economy = ServiceProvider.Get<EconomyService>();
      economy.ProcessingOre.Subscribe(ProcessingOreChanged).AddTo(gameObject);
      economy.ProcessingStage.Subscribe(StageChanged).AddTo(gameObject);
    }

    private void ProcessingOreChanged(ulong amount)
    {
      var text = new TextData(amount > 0 ? "processing_ore" : "no_ore");
      title.SetText(text);
      processingOreAmountText.text = MoneyFormatter.Format((long)amount);
    }

    private void StageChanged(int stage)
    {
      var amount = ServiceProvider.Get<OreProcessingService>().GetOrePerSecond(stage);
      stageOreAmountText.SetText(new TextData("per_second", MoneyFormatter.Format(amount)));
    }
  }
}