using Game.Scripts.Infrastructure.Services;
using UnityEngine;

namespace Game.Scripts.Tutorial
{
  public class TutorialTargetView : MonoBehaviour
  {
    [SerializeField] private MonoBehaviour target;

    private void Awake()
    {
      ServiceProvider.Get<TutorialService>().RegisterTarget(target);
    }
  }
}