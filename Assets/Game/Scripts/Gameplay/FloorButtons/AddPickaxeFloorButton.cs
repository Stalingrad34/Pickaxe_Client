using Game.Scripts.Gameplay.OreMining;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.Database;
using Game.Scripts.UI;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.FloorButtons
{
  public class AddPickaxeFloorButton : MonoBehaviour
  {
    [SerializeField] private int pickaxesCount;
    [SerializeField] private CustomText title;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject onRoot;
    [SerializeField] private GameObject offRoot;

    private DatabaseService _databaseService;

    private void Start()
    {
      canvas.worldCamera = UIManager.GameCamera;
      title.SetText(new TextData("buy", pickaxesCount.ToString()));
      
      _databaseService = ServiceProvider.Get<DatabaseService>();
      _databaseService.PickaxesNominal.Subscribe(UpdateVisual).AddTo(gameObject);
      _databaseService.Money.Subscribe(UpdateVisual).AddTo(gameObject);
    }

    private void UpdateVisual(ulong nominal)
    {
      var pickaxeCost = ServiceProvider.Get<OreMiningService>().GetPickaxeCost(_databaseService.PickaxesNominal.Value) * (ulong)pickaxesCount;
      onRoot.SetActive(_databaseService.Money.Value >= pickaxeCost);
      offRoot.SetActive(_databaseService.Money.Value < pickaxeCost);
      cost.text = "$" + MoneyFormatter.Format((long)pickaxeCost);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        ServiceProvider.Get<OreMiningService>().TryAddPickaxes(pickaxesCount);
      }
    }
  }
}