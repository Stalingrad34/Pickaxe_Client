using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.KinematicCharacter.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.KinematicCharacter.Aspects
{
  public class KinematicCharacterAspect : IProtoAspect
  {
    public ProtoPool<KinematicCharacterComponent> Kinematics;
    public ProtoPool<CharacterAnimatorComponent> Animators;
    private ProtoWorld _world;
    
    public void Init(ProtoWorld world)
    {
      _world = world;
      _world.AddAspect(this);
      
      Kinematics = new ProtoPool<KinematicCharacterComponent>();
      _world.AddPool(Kinematics);
      
      Animators = new ProtoPool<CharacterAnimatorComponent>();
      _world.AddPool(Animators);
    }

    public void PostInit()
    {
      
    }

    public ProtoWorld World()
    {
      return _world;
    }
  }
}