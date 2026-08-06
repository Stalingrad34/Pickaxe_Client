using Game.Scripts.Infrastructure.Services;
using UniRx;
using UnityEngine;

namespace Game.Scripts.Gameplay.FloorButtons
{
  public class ReviewFloorButton : MonoBehaviour
  {
    private void Awake()
    {
      ServiceProvider.Get<ReviewService>().IsAvailable.Subscribe(gameObject.SetActive).AddTo(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        ServiceProvider.Get<ReviewService>().OpenReview();
      }
    }
  }
}