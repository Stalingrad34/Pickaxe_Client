using Game.Scripts.Gameplay.ECS.Pickup.Interfaces;
using UnityEngine;

namespace Game.Scripts.Gameplay.Chest
{
  public class ChestView : MonoBehaviour, IPickupItem
  {
    [SerializeField] private Rigidbody rb;
    public Transform Transform => transform;
    
    public void Init(ChestData data)
    {
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
  }
}