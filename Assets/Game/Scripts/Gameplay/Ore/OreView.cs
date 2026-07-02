using UnityEngine;

namespace Game.Scripts.Gameplay.Ore
{
  public class OreView : MonoBehaviour
  {
    public void Init(OreData data)
    {
      var setupComponents = gameObject.GetComponents<IOreSetup>();
      foreach (var setupComponent in setupComponents)
      {
        setupComponent.Setup(data);
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        
      }
    }
  }
}