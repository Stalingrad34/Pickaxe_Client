using System;
using Core.Scripts.Loggers;
using Game.Scripts.Gameplay.ECS.Camera;
using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.EntityConvert;
using Game.Scripts.Gameplay.ECS.Input;
using Game.Scripts.Gameplay.ECS.KinematicCharacter;
using Game.Scripts.Gameplay.ECS.Pickaxe;
using Game.Scripts.Gameplay.ECS.RigidBody;
using Game.Scripts.Gameplay.ECS.Spawn;
using Game.Scripts.KinematicCharacterController.ExampleCharacter.Scripts;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS
{
  public class ECSRunner : MonoBehaviour
  {
    public static ECSEventWriter EcsEventWriter { get; private set; }
    public ProtoWorld World => _world;
    
    [SerializeField] private ExampleCharacterCamera mainCamera;
    
    private MainAspect _mainAspect;
    private ProtoWorld _world;
    private IProtoSystems _updateSystems;
    private IProtoSystems _fixedSystems;
    private IProtoSystems _lateSystems;
    
    private bool _hasException;

    private void Awake()
    {
      var config = new ProtoWorld.Config()
      {
        Entities = 1024,
        Pools = 64,
        Aspects = 32,
        RecycledEntities = 1024
      };
      
      var inputModule = new InputModule();
      var spawnModule = new SpawnModule();
      var cameraModule = new CameraModule();
      var moveModule = new KinematicCharacterModule();
      var rigidbodyModule = new RigidbodyModule();
      var convertEntityModule = new ConvertEntityModule();
      var pickaxeModule = new PickaxeModule();
      
      _mainAspect = new MainAspect();
      _mainAspect.AddAspects(inputModule.Aspects());
      _mainAspect.AddAspects(spawnModule.Aspects());
      _mainAspect.AddAspects(cameraModule.Aspects());
      _mainAspect.AddAspects(moveModule.Aspects());
      _mainAspect.AddAspects(rigidbodyModule.Aspects());
      _mainAspect.AddAspects(convertEntityModule.Aspects());
      _mainAspect.AddAspects(pickaxeModule.Aspects());
      
      _world = new ProtoWorld(_mainAspect, config);
      EcsEventWriter = new ECSEventWriter(_world);
      
      var inputActions = new InputActions();
      inputActions.Enable();
      
      _updateSystems = new ProtoSystems(_world);
      _updateSystems
        .AddService(mainCamera, typeof(ExampleCharacterCamera))
        .AddService(inputActions, typeof(InputActions))
        .AddModule(convertEntityModule)
        .AddModule(inputModule)
        .AddModule(spawnModule)
        .AddModule(moveModule)
        .AddModule(pickaxeModule)
        //.AddSystem(new OneFrameSystem<PlayerChangeEvent>(_mainAspect.Events.PlayerChangeEvents))
        .Init();
      
      _fixedSystems = new ProtoSystems(_world);
      _fixedSystems
        .AddModule(rigidbodyModule)
        .Init();
      
      _lateSystems = new ProtoSystems(_world);
      _lateSystems
        .AddModule(cameraModule)
        .Init();
    }

    private void Update() => Run(_updateSystems);

    private void FixedUpdate() => Run(_fixedSystems);

    private void LateUpdate() => Run(_lateSystems);

    private void Run(IProtoSystems systems)
    { 
      if (_hasException)
        return;

      try
      {
        systems?.Run();
      }
      catch (Exception e)
      {
        _hasException = true;
        ECSLogger.ShowLogs();
        throw new UnityException(e.ToString());
      }
    }

    private void OnDestroy()
    {
      _updateSystems?.Destroy();
      _fixedSystems?.Destroy();
      _lateSystems?.Destroy();
      _world?.Destroy();
    }
  }
}
