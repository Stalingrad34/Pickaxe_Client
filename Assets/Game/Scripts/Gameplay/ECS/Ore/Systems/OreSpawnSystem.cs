using Game.Scripts.Gameplay.ECS.Ore.Aspects;
using Game.Scripts.Gameplay.ECS.Ore.Components;
using Game.Scripts.Gameplay.Ore;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Ore.Systems
{
  public class OreSpawnSystem : IProtoInitSystem, IProtoRunSystem
  {
    private OreAspect _oreAspect;
    private IProtoIt _eventEntities;

    public void Init(IProtoSystems systems)
    {
      _oreAspect = systems.GetAspect<OreAspect>();
      _eventEntities = Entities.ProtoIt<OreSpawnEvent>(systems.World());
    }

    public void Run()
    {
      foreach (var eventEntity in _eventEntities)
      {
        var spawnEvent = _oreAspect.OreSpawnPool.Get(eventEntity);
        var oreView = Object.Instantiate(spawnEvent.Config.prefab, spawnEvent.Position, Quaternion.identity);
        var oreData = new OreData()
        {
          StartForce = spawnEvent.Direction
        };
        oreView.Init(oreData);
      }
    }
  }
}