using Game.Scripts.Gameplay.ECS.Input.Aspects;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Converters
{
  public class ControlConverter : MonoBehaviour, IEntityConverter
  {
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var inputAspect = systems.GetAspect<InputAspect>();
      inputAspect.Controls.Add(entity);
    }
  }
}