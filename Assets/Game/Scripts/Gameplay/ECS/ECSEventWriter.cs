using System.Collections.Generic;
using Game.Scripts.Gameplay.ECS.Character.Aspects;
using Game.Scripts.Gameplay.ECS.EntityConvert.Aspects;
using Game.Scripts.Gameplay.ECS.Ore.Aspects;
using Game.Scripts.Gameplay.ECS.Pickaxe.Aspects;
using Game.Scripts.Gameplay.Ore;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Gameplay.Units;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS
{
  public class ECSEventWriter
  {
    private readonly CharacterAspect _characterAspect;
    private readonly OreAspect _oreAspect;
    private readonly PickaxeAspect _pickaxeAspect;
    private readonly EntityConvertAspect _entityConvert;

    public ECSEventWriter(ProtoWorld world)
    {
      _characterAspect = world.GetAspect<CharacterAspect>();
      _oreAspect = world.GetAspect<OreAspect>();
      _pickaxeAspect = world.GetAspect<PickaxeAspect>();
      _entityConvert = world.GetAspect<EntityConvertAspect>();
    }

    public void SpawnCharacter(UnitData unitData, string prefabName)
    {
      ref var evt = ref _characterAspect.CharacterSpawnPool.NewEntity(out _);
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
      ref var oreSpawn = ref _oreAspect.OreSpawnPool.NewEntity(out _);
      oreSpawn.Position = position;
      oreSpawn.Direction = direction;
      oreSpawn.Config = oreConfig;
    }
  }
}