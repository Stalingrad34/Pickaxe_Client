using Game.Scripts.Gameplay.ECS.Pickup.Interfaces;
using Game.Scripts.Infrastructure.Services;
using UnityEngine;

namespace Game.Scripts.Gameplay.Chest
{
  public class ChestView : MonoBehaviour, IPickupItem
  {
    [SerializeField] private Rigidbody rb;
    private ChestData _data;
    public Transform Transform => transform;
    
    public void Init(ChestData data)
    {
      _data = data;
      var setupComponents = gameObject.GetComponents<IChestSetup>();
      foreach (var setupComponent in setupComponents)
      {
        setupComponent.Setup(data);
      }
    }
    
    public void Pickup()
    {
      transform.localRotation = Quaternion.Euler(0, 75 , 0);
      rb.isKinematic = true;
    }

    public void Discarded()
    {
      ServiceProvider.Get<AnalyticsService>().MetricaSend("chest", "Discard", _data.Config.Type.ToString());
      Destroy(gameObject);
    }
  }
}