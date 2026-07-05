using Game.Scripts.Gameplay.ECS.EntityConvert.Converters;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Common.Converters
{
  public class TransformConverter : MonoBehaviour, IEntityConverter
  {
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<TransformAspect>();
      ref var component = ref aspect.TransformsPool.Add(entity);
      component.Transform = transform;
    }
  }
}