using System;
using Game.Scripts.Infrastructure.Services;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.FloorButtons
{
  public class ProcessOreFloorButton : MonoBehaviour
  {
    [SerializeField] private GameObject onRoot;
    [SerializeField] private GameObject offRoot;

    private OreProcessingService _oreProcessing;
    private EconomyService _economy;

    private void Start()
    {
      _oreProcessing = ServiceProvider.Get<OreProcessingService>();
      _economy = ServiceProvider.Get<EconomyService>();
      _economy.Ore.Subscribe(OreChanged).AddTo(gameObject);
    }

    private void OreChanged(ulong amount)
    {
      onRoot.SetActive(amount > 0);
      offRoot.SetActive(amount == 0);
    }
    
    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        _oreProcessing.ProcessOre();
      }
    }
  }
}