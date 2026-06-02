using System;
using Game.Scripts.Gameplay.ECS.Common;
using Game.Scripts.Gameplay.ECS.EntityConvert.Aspects;
using Game.Scripts.Gameplay.ECS.EntityConvert.Components;
using Game.Scripts.Gameplay.ECS.EntityConvert.Systems;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.EntityConvert
{
  public class ConvertEntityModule : IProtoModule
  {
    public void Init(IProtoSystems systems)
    {
      var instantiateAspect = systems.GetAspect<EntityConvertAspect>();
      systems
        .AddSystem(new EntityConvertSystem())
        .AddSystem(new OneFrameSystem<EntityConvertEvent>(instantiateAspect.Events));
    }

    public IProtoAspect[] Aspects()
    {
      return new IProtoAspect[]
      {
        new EntityConvertAspect()
      };
    }

    public Type[] Dependencies()
    {
      return null;
    }
  }
}