using System;
using Game.Scripts.Infrastructure.Services;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.FloorButtons
{
  public class MergePickaxesFloorButton : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject onRoot;
    [SerializeField] private GameObject offRoot;
    
    private PickaxesService _pickaxesService;
    
    private void Start()
    {
      _pickaxesService = ServiceProvider.Get<PickaxesService>();
      _pickaxesService.CanMerge.Subscribe(CanMergeChanged).AddTo(gameObject);
    }

    private void CanMergeChanged(bool canMerge)
    {
      onRoot.SetActive(canMerge);
      offRoot.SetActive(!canMerge);
      title.gameObject.SetActive(canMerge);
    }
    
    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        _pickaxesService.TryMergeAll();
      }
    }
  }
}