using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.UI;
using Game.Scripts.UI;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.FloorButtons
{
  public class AddPickaxeFloorButton : MonoBehaviour
  {
    [SerializeField] private int pickaxesCount;
    [SerializeField] private bool autoMerge;
    [SerializeField] private CustomText title;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject onRoot;
    [SerializeField] private GameObject offRoot;

    private EconomyService _economy;
    private PickaxesService _pickaxes;

    private void Start()
    {
      canvas.worldCamera = UIManager.GameCamera;
      title.SetText(new TextData("buy_pickaxes", pickaxesCount.ToString()));
      
      _economy = ServiceProvider.Get<EconomyService>();
      _pickaxes = ServiceProvider.Get<PickaxesService>();
      
      _pickaxes.PickaxesNominal.Subscribe(UpdateVisual).AddTo(gameObject);
      _economy.Money.Subscribe(UpdateVisual).AddTo(gameObject);
    }

    private void UpdateVisual(ulong nominal)
    {
      var pickaxeCost = _pickaxes.GetPickaxeCost() * (ulong)pickaxesCount;
      onRoot.SetActive(_economy.Money.Value >= pickaxeCost);
      offRoot.SetActive(_economy.Money.Value < pickaxeCost);
      cost.text = "$" + MoneyFormatter.Format((long)pickaxeCost);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        _pickaxes.TryAddPickaxes(pickaxesCount, autoMerge);
      }
    }
  }
}