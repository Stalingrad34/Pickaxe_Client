using Game.Scripts.Gameplay.ECS.EntityConvert.Converters;
using Game.Scripts.Gameplay.ECS.RigidBody.Aspects;
using Game.Scripts.Gameplay.Ore;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Spawn.Converters
{
  public class OreConverter : MonoBehaviour, IEntityConverter, IOreSetup
  {
    [SerializeField] private OreView oreView;
    private Vector3 _startForce;
    
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var oreAspect = systems.GetAspect<SpawnAspect>();
      ref var component = ref oreAspect.OrePool.Add(entity);
      component.OreView = oreView;

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