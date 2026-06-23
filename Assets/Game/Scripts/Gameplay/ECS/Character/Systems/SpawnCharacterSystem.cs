using Game.Scripts.Gameplay.ECS.Character.Aspects;
using Game.Scripts.Gameplay.ECS.Pickaxe.Aspects;
using Game.Scripts.Gameplay.ECS.Pickaxe.Components;
using Game.Scripts.Gameplay.ECS.Spawn.Components;
using Game.Scripts.Infrastructure;
using Game.Scripts.KinematicCharacterController.ExampleCharacter.Scripts;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Character.Systems
{
  public class SpawnCharacterSystem : IProtoInitSystem, IProtoRunSystem
  {
    private ExampleCharacterCamera _mainCamera;
    private CharacterAspect _characterAspect;
    private PickaxeAspect _pickaxeAspect;
    private ProtoIt _spawnEntities;
    private ProtoIt _mineEntities;
    
    public void Init(IProtoSystems systems)
    {
      _mainCamera = systems.GetService<ExampleCharacterCamera>();
      _characterAspect = systems.GetAspect<CharacterAspect>();
      _pickaxeAspect = systems.GetAspect<PickaxeAspect>();
      _spawnEntities = Entities.ProtoIt<SpawnCharacterEvent>(systems.World());
      _mineEntities = Entities.ProtoIt<PickaxeMineComponent>(systems.World());
    }
    
    public void Run()
    {
      foreach (var entity in _spawnEntities)
      {
        ref var spawnEvent = ref _characterAspect.SpawnCharacterEvents.Get(entity);
        foreach (var mineEntity in _mineEntities)
        {
          ref var mine = ref _pickaxeAspect.PickaxeMines.Get(mineEntity);
          if (!string.IsNullOrEmpty(mine.OwnerId))
            continue;

          mine.OwnerId = spawnEvent.Data.Id;
          spawnEvent.Data.Position = mine.PlayerSpawnPosition.position;
          spawnEvent.Data.StartAngleY = mine.PlayerSpawnPosition.eulerAngles.y;
          break;
        }
        
        var characterView = AssetProvider.GetCharacterView(spawnEvent.PrefabPath);
        characterView.Setup(spawnEvent.Data, _mainCamera);
      }
    }
  }
}