using Game.Scripts.Gameplay.ECS.RigidBody.Aspects;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Converters
{
  public class RigidbodyConverter : MonoBehaviour, IEntityConverter
  {
    [SerializeField] private Rigidbody rb;
    
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<RigidbodyAspect>();
      ref var component = ref aspect.Rigidbodies.Add(entity);
      component.Rigidbody = rb;
    }
  }
}