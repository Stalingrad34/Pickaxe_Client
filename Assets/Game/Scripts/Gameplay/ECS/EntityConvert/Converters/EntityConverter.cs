using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Converters
{
  public class EntityConverter : MonoBehaviour
  {
    private void Start()
    {
      ECSRunner.EcsEventWriter.EntityConvert(gameObject);
    }
  }
}