using Game.Scripts.Gameplay.ECS.EntityConvert.Converters;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Destroy.Converters
{
  public class DestroyPoolItemConverter : MonoBehaviour, IEntityConverter
  {
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<DestroyAspect>();
      aspect.DestroyPool.Add(entity);
    }
  }
}