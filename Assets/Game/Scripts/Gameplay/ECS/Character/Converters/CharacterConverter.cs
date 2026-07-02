using Game.Scripts.Gameplay.ECS.Character.Aspects;
using Game.Scripts.Gameplay.ECS.EntityConvert.Converters;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Character.Converters
{
  public class CharacterConverter : MonoBehaviour, IEntityConverter
  {
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<CharacterAspect>();
      ref var component = ref aspect.CharacterComponentPool.Add(entity);
      component.Transform = transform;
    }
  }
}