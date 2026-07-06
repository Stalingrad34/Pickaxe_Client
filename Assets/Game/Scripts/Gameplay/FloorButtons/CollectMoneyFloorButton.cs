using Game.Scripts.Infrastructure.Services;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.FloorButtons
{
  public class CollectMoneyFloorButton : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI processingMoneyText;
    [SerializeField] private GameObject onRoot;
    [SerializeField] private GameObject offRoot;

    private EconomyService _economy;
    
    private void Start()
    {
      _economy = ServiceProvider.Get<EconomyService>();
      _economy.ProcessingMoney.Subscribe(ProcessingMoneyChanged).AddTo(gameObject);
    }

    private void ProcessingMoneyChanged(ulong amount)
    {
      onRoot.SetActive(amount > 0);
      offRoot.SetActive(amount == 0);
      processingMoneyText.text = $"${MoneyFormatter.Format((long)amount)}";
    }
    
    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        _economy.CollectMoney();
      }
    }
  }
}