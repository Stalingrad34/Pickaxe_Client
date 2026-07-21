using UnityEngine;

namespace Game.Scripts.Gameplay.Chest
{
  public class ChestView : MonoBehaviour
  {
    public void Init(ChestData data)
    {
      var setupComponents = gameObject.GetComponents<IChestSetup>();
      foreach (var setupComponent in setupComponents)
      {
        setupComponent.Setup(data);
      }
    }
  }
}