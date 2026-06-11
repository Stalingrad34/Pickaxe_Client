using System.Collections.Generic;
using Game.Scripts.Gameplay.ECS.EntityConvert.Aspects;
using Game.Scripts.Gameplay.ECS.Pickaxe.Aspects;
using Game.Scripts.Gameplay.ECS.Spawn.Aspects;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Gameplay.Units;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS
{
  public class ECSEventWriter
  {
    private readonly SpawnAspect _spawn;
    private readonly PickaxeAspect _pickaxeAspect;
    private readonly EntityConvertAspect _entityConvert;

    public ECSEventWriter(ProtoWorld world)
    {
      _spawn = world.GetAspect<SpawnAspect>();
      _pickaxeAspect = world.GetAspect<PickaxeAspect>();
      _entityConvert = world.GetAspect<EntityConvertAspect>();
    }

    public void SpawnCharacter(UnitData unitData, string prefabName)
    {
      ref var evt = ref _spawn.SpawnCharacterEvents.NewEntity(out _);
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
  }
}