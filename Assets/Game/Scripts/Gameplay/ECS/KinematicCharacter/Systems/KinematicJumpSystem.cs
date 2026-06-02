using System;
using Game.Scripts.Gameplay.ECS.Input.Aspects;
using Game.Scripts.Gameplay.ECS.Input.Components;
using Game.Scripts.Gameplay.ECS.KinematicCharacter.Aspects;
using Game.Scripts.Gameplay.ECS.KinematicCharacter.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.KinematicCharacter.Systems
{
  public class KinematicJumpSystem : IProtoInitSystem, IProtoRunSystem
  {
    private InputAspect _input;
    private KinematicCharacterAspect _character;
    private ProtoIt _entities;
    
    public void Init(IProtoSystems systems)
    {
      _input = systems.GetAspect<InputAspect>();
      _character = systems.GetAspect<KinematicCharacterAspect>();
      _entities = Entities.ProtoIt<ControlComponent, KinematicCharacterComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _entities)
      {
        
      }
    }
  }
}