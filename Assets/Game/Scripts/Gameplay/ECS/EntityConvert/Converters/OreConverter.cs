using Game.Scripts.Gameplay.ECS.Ore.Aspects;
using Game.Scripts.Gameplay.ECS.RigidBody.Aspects;
using Game.Scripts.Gameplay.Ore;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Converters
{
  public class OreConverter : MonoBehaviour, IEntityConverter, IOreSetup
  {
    private Vector3 _startForce;
    
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var oreAspect = systems.GetAspect<OreAspect>();
      oreAspect.OrePool.Add(entity);

      var rigidbodyAspect = systems.GetAspect<RigidbodyAspect>();
      ref var forceEvent = ref rigidbodyAspect.AddForceEvents.Add(entity);
      forceEvent.Force = _startForce;
    }

    public void Setup(OreData data)
    {
      _startForce = data.StartForce;
    }
  }
}