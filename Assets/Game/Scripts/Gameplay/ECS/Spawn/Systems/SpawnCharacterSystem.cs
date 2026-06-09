using Game.Scripts.Gameplay.ECS.Spawn.Aspects;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Game.Scripts.Infrastructure;
using Game.Scripts.KinematicCharacterController.ExampleCharacter.Scripts;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Spawn.Systems
{
  public class SpawnCharacterSystem : IProtoInitSystem, IProtoRunSystem
  {
    private ExampleCharacterCamera _mainCamera;
    private SpawnAspect _spawn;
    private ProtoIt _entities;
    
    public void Init(IProtoSystems systems)
    {
      _spawn = systems.GetAspect<SpawnAspect>();
      _mainCamera = systems.GetService<ExampleCharacterCamera>();
      _entities = Entities.ProtoIt<SpawnCharacterEvent>(systems.World());
    }
    
    public void Run()
    {
      foreach (var entity in _entities)
      {
        var spawnEvent = _spawn.SpawnCharacterEvents.Get(entity);
        
        var characterView = AssetProvider.GetCharacterView(spawnEvent.PrefabPath);
        characterView.Setup(spawnEvent.Data, _mainCamera);
      }
    }
  }
}