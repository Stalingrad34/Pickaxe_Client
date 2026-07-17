using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Services;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.FloorButtons
{
  public class ProcessStageFloorButton : MonoBehaviour
  {
    [SerializeField] private CustomText nextStageText;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private GameObject onRoot;
    [SerializeField] private GameObject offRoot;

    private EconomyService _economy;
    private int _stage;

    private void Start()
    {
      _economy = ServiceProvider.Get<EconomyService>();
      _economy.ProcessingStage.Subscribe(ProcessingStageChanged).AddTo(gameObject);
      _economy.Money.Subscribe(MoneyChanged).AddTo(gameObject);
    }

    private void ProcessingStageChanged(int stage)
    {
      _stage = stage;
      var processingData = ServiceProvider.Get<OreProcessingService>().GetOrePrecessingData(_stage + 1);
      var oreAmount = ServiceProvider.Get<OreProcessingService>().GetOrePerSecond(_stage);
      var boost = processingData.OreCount - oreAmount;
      nextStageText.SetText(new TextData("per_second", $"+{boost}"));
      cost.text = $"${MoneyFormatter.Format(processingData.UpgradeCost)}";
    }

    private void MoneyChanged(ulong money)
    {
      var processingData = ServiceProvider.Get<OreProcessingService>().GetOrePrecessingData(_stage + 1);
      var canBoost = money >= (ulong)processingData.UpgradeCost;
      onRoot.SetActive(canBoost);
      offRoot.SetActive(!canBoost);
    }
    
    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        var processingData = ServiceProvider.Get<OreProcessingService>().GetOrePrecessingData(_stage + 1);
        _economy.IncreaseStage((ulong)processingData.UpgradeCost);
      }
    }
  }
}