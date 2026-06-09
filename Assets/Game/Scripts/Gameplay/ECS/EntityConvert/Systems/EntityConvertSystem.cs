using Game.Scripts.Gameplay.ECS.EntityConvert.Aspects;
using Game.Scripts.Gameplay.ECS.EntityConvert.Components;
using Game.Scripts.Gameplay.ECS.EntityConvert.Converters;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Systems
{
  public class EntityConvertSystem : IProtoInitSystem, IProtoRunSystem
  {
    private EntityConvertAspect _entityConvertAspect;
    private IProtoSystems _systems;
    private ProtoIt _entities;
    
    public void Init(IProtoSystems systems)
    {
      _systems = systems;
      _entityConvertAspect = systems.GetAspect<EntityConvertAspect>();
      _entities = Entities.ProtoIt<EntityConvertEvent>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _entities)
      {
        var gameObject = _entityConvertAspect.Events.Get(entity).GameObject;
        if (!gameObject.TryGetComponent<EntityConverter>(out var convert))
          continue;

        var converters = gameObject.GetComponents<IEntityConverter>();
        foreach (var converter in converters)
        {
          converter.Convert(entity, _systems);
        }
        
        convert.SetEntity(entity);
        _entityConvertAspect.Events.Del(entity);
      }
    }
  }
}