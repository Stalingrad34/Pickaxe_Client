using Game.Scripts.Gameplay.ECS.Character.Aspects;
using Game.Scripts.Gameplay.ECS.Character.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Character.Systems
{
  public class CharacterPickupSystem : IProtoInitSystem, IProtoRunSystem
  {
    private CharacterAspect _characterAspect;
    private ProtoIt _characterEntities;
    private ProtoIt _pickupEntities;
    
    public void Init(IProtoSystems systems)
    {
      _characterAspect = systems.GetAspect<CharacterAspect>();
      _characterEntities = Entities.ProtoIt<CharacterComponent>(systems.World());
      _pickupEntities = Entities.ProtoIt<CharacterPickupComponent>(systems.World());
    }

    public void Run()
    {
      foreach (var characterEntity in _characterEntities)
      {
        
      }
    }
  }
}