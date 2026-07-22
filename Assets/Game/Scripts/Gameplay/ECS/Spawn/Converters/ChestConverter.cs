using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Gameplay.ECS.EntityConvert.Converters;
using Game.Scripts.Gameplay.ECS.RigidBody.Aspects;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Spawn.Converters
{
  public class ChestConverter : MonoBehaviour, IEntityConverter, IChestSetup
  {
    [SerializeField] private ChestView view;
    
    private ChestConfig _chestConfig;
    private Vector3 _startForce;

    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<SpawnAspect>();
      ref var component = ref aspect.ChestPool.Add(entity);
      component.ChestConfig = _chestConfig;
      component.ChestView = view;
      
      var rigidbodyAspect = systems.GetAspect<RigidbodyAspect>();
      ref var forceEvent = ref rigidbodyAspect.AddForceEvents.Add(entity);
      forceEvent.Force = _startForce;
    }

    public void Setup(ChestData chestData)
    {
      _chestConfig = chestData.Config;
      _startForce = chestData.StartForce;
    }
  }
}