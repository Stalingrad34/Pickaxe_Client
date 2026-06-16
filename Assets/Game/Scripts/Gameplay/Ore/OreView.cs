using UnityEngine;

namespace Game.Scripts.Gameplay.Ore
{
  public class OreView : MonoBehaviour
  {
    private OreConfig _config;

    public void Init(OreConfig config)
    {
      _config = config;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        
      }
    }
  }
}