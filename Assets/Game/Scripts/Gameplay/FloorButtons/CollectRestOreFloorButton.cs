using Game.Scripts.Infrastructure.Services;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.FloorButtons
{
  public class CollectRestOreFloorButton : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI restOreCount;
    
    private EconomyService _economyService;

    private void Awake()
    {
      _economyService = ServiceProvider.Get<EconomyService>();
      _economyService.RestOre.Subscribe(RestOreChanged).AddTo(gameObject);
    }

    private void RestOreChanged(ulong amount)
    {
      gameObject.SetActive(amount > 0);
      restOreCount.text = MoneyFormatter.Format((long)amount);
    }
    
    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        _economyService.ConvertRestOre();
      }
    }
  }
}