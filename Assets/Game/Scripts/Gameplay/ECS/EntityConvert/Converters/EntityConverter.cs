using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Converters
{
  public class EntityConverter : MonoBehaviour
  {
    private ProtoEntity? _entity;
    
    private void Start()
    {
      ECSRunner.EcsEventWriter.EntityConvert(gameObject);
    }

    public void SetEntity(ProtoEntity entity)
    {
      _entity = entity;
    }

    public bool TryGetEntity(out ProtoEntity entity)
    {
      if (_entity.HasValue)
      {
        entity = _entity.Value;
        return true;
      }

      entity = default;
      return false;
    }
  }
}