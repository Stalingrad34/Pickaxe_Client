using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Game.Scripts.Gameplay.Ore;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Spawn.Systems
{
  public class SpawnOreSystem : IProtoInitSystem, IProtoRunSystem
  {
    private SpawnAspect _spawnAspect;
    private ProtoIt _eventEntities;

    public void Init(IProtoSystems systems)
    {
      _spawnAspect = systems.GetAspect<SpawnAspect>();
      _eventEntities = Entities.ProtoIt<SpawnOreEvent>(systems.World());
    }

    public void Run()
    {
      foreach (var eventEntity in _eventEntities)
      {
        var spawnEvent = _spawnAspect.SpawnOrePool.Get(eventEntity);
        var oreView = Object.Instantiate(spawnEvent.Config.prefab, spawnEvent.Position, Quaternion.identity);
        var oreData = new OreData()
        {
          StartForce = spawnEvent.Direction,
          Amount = (ulong)spawnEvent.Config.miningCount,
          PickupTextColor = spawnEvent.Config.pickupColor,
        };
        oreView.Init(oreData);
      }
    }
  }
}