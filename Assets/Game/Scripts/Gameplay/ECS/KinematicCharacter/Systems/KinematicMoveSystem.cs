using Game.Scripts.Gameplay.ECS.Camera.Aspects;
using Game.Scripts.Gameplay.ECS.Camera.Components;
using Game.Scripts.Gameplay.ECS.Input.Aspects;
using Game.Scripts.Gameplay.ECS.Input.Components;
using Game.Scripts.Gameplay.ECS.KinematicCharacter.Aspects;
using Game.Scripts.Gameplay.ECS.KinematicCharacter.Components;
using Game.Scripts.KinematicCharacterController.Core;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.KinematicCharacter.Systems
{
  public class KinematicMoveSystem : IProtoInitSystem, IProtoRunSystem
  {
    private InputAspect _input;
    private KinematicCharacterAspect _character;
    private CameraAspect _camera;
    private ProtoIt _entities;
    private ProtoIt _cameraEntities;
    
    public void Init(IProtoSystems systems)
    {
      _input = systems.GetAspect<InputAspect>();
      _character = systems.GetAspect<KinematicCharacterAspect>();
      _camera = systems.GetAspect<CameraAspect>();
      
      _entities = Entities.ProtoIt<ControlComponent, KinematicCharacterComponent>(systems.World());
      _cameraEntities = Entities.ProtoIt<CameraComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _entities)
      {
        var control = _input.Controls.Get(entity);
        var kinematic = _character.Kinematics.Get(entity);
        var input = new PlayerCharacterInputs()
        {
          MoveAxisForward = control.VerticalAxis,
          MoveAxisRight = control.HorizontalAxis,
          CameraRotation = _camera.Cameras.Get(_cameraEntities.FirstSlow().Entity).Camera.Transform.rotation,
          JumpDown = control.SpaceKeyDown
        };
        
        kinematic.Processor.SetInputs(ref input);
      }
    }
  }
}