using System.Collections.Generic;
using Game.Scripts.Gameplay.Chest;
using Game.Scripts.Gameplay.ECS.EntityConvert.Aspects;
using Game.Scripts.Gameplay.ECS.Pickaxe.Aspects;
using Game.Scripts.Gameplay.ECS.Spawn;
using Game.Scripts.Gameplay.Ore;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Gameplay.Units;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS
{
  public class ECSEventWriter
  {
    private readonly SpawnAspect _spawnAspect;
    private readonly PickaxeAspect _pickaxeAspect;
    private readonly EntityConvertAspect _entityConvert;

    public ECSEventWriter(ProtoWorld world)
    {
      _spawnAspect = world.GetAspect<SpawnAspect>();
      _pickaxeAspect = world.GetAspect<PickaxeAspect>();
      _entityConvert = world.GetAspect<EntityConvertAspect>();
    }

    public void SpawnCharacter(UnitData unitData, string prefabName)
    {
      ref var evt = ref _spawnAspect.SpawnCharacterPool.NewEntity(out _);
      evt.Data = unitData;
      evt.PrefabPath = prefabName;
    }
    
    public void EntityConvert(GameObject gameObject)
    {
      ref var evt = ref _entityConvert.Events.NewEntity(out _);
      evt.GameObject = gameObject;
    }

    public void RebuildPickaxes(string ownerId, Dictionary<PickaxeType, int> pickaxes)
    {
      ref var spawnEvent = ref _pickaxeAspect.RebuildPickaxeEvents.NewEntity(out _);
      spawnEvent.OwnerID = ownerId;
      spawnEvent.Pickaxes = pickaxes;
    }

    public void PickaxesPunch(string ownerId)
    {
      ref var punchEvent = ref _pickaxeAspect.PickaxesPunchEvents.NewEntity(out _);
      punchEvent.OwnerId =  ownerId;
    }

    public void SpawnOre(Vector3 position, Vector3 direction, OreConfig oreConfig)
    {
      ref var oreSpawn = ref _spawnAspect.SpawnOrePool.NewEntity(out _);
      oreSpawn.Position = position;
      oreSpawn.Direction = direction;
      oreSpawn.Config = oreConfig;
    }
    
    public void SpawnChest(Vector3 position, Vector3 direction, ChestConfig chestConfig)
    {
      ref var oreSpawn = ref _spawnAspect.SpawnChestPool.NewEntity(out _);
      oreSpawn.Position = position;
      oreSpawn.Direction = direction;
      oreSpawn.Config = chestConfig;
    }
  }
}