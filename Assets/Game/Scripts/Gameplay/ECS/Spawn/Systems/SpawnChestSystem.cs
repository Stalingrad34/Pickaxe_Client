using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Spawn.Systems
{
  public class SpawnChestSystem : IProtoInitSystem, IProtoRunSystem
  {
    private SpawnAspect _spawnAspect;
    private ProtoIt _eventEntities;

    public void Init(IProtoSystems systems)
    {
      _spawnAspect = systems.GetAspect<SpawnAspect>();
      _eventEntities = Entities.ProtoIt<SpawnChestEvent>(systems.World());
    }

    public void Run()
    {
      foreach (var eventEntity in _eventEntities)
      {
        var spawnEvent = _spawnAspect.SpawnChestPool.Get(eventEntity);
        var oreView = Object.Instantiate(spawnEvent.Config.Prefab, spawnEvent.Position, Quaternion.identity);
        var oreData = new ChestData()
        {
          StartForce = spawnEvent.Direction
        };
        oreView.Init(oreData);
      }
    }
  }
}