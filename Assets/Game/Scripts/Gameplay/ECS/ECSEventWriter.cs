using Game.Scripts.Gameplay.Data.Units;
using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.EntityConvert.Aspects;
using Game.Scripts.Gameplay.ECS.Spawn.Aspects;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS
{
  public class ECSEventWriter
  {
    private readonly EventsAspect _events;
    private readonly SpawnAspect _spawn;
    private readonly EntityConvertAspect _entityConvert;

    public ECSEventWriter(ProtoWorld world)
    {
      _events = world.GetAspect<EventsAspect>();
      _spawn = world.GetAspect<SpawnAspect>();
      _entityConvert = world.GetAspect<EntityConvertAspect>();
    }

    public void SpawnCharacter(UnitData unitData, string prefabName)
    {
      ref var evt = ref _spawn.SpawnEvents.NewEntity(out _);
      evt.Data = unitData;
      evt.PrefabPath = prefabName;
    }
    
    public void EntityConvert(GameObject gameObject)
    {
      ref var evt = ref _entityConvert.Events.NewEntity(out _);
      evt.GameObject = gameObject;
    }

    public void PlayerChanged()
    {
      _events.PlayerChangeEvents.NewEntity(out _);
    }
  }
}