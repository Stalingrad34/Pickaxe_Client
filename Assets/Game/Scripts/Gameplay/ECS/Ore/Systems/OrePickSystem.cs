using Game.Scripts.Gameplay.ECS.Ore.Aspects;
using Game.Scripts.Gameplay.ECS.Ore.Components;
using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Ore.Systems
{
  public class OrePickSystem : IProtoInitSystem, IProtoRunSystem
  {
    private OreAspect _oreAspect;
    private ProtoIt _oreEntities;

    public void Init(IProtoSystems systems)
    {
      _oreAspect = systems.GetAspect<OreAspect>();
      _oreEntities = Entities.ProtoIt<OreComponent>(systems.World());
    }

    public void Run()
    {
      
    }
  }
}