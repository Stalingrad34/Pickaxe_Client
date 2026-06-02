using System;
using BitGames.Bits;
using Game.Scripts.Gameplay.ECS.Input.Aspects;
using Game.Scripts.Gameplay.ECS.Input.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Input
{
  public class InputModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      if ((Platform.IsWebGL() && !Platform.IsMobileWebGL()) || Platform.IsEditor())
        systems.AddSystem(new KeysAxisSystem());
      else
        systems.AddSystem(new JoystickAxisSystem());

      systems.AddSystem(new TouchSystem());
    }

    public IProtoAspect[] Aspects()
    {
      return new IProtoAspect[]
      {
        new InputAspect()
      };
    }

    public Type[] Dependencies()
    {
      return null;
    }
  }
}