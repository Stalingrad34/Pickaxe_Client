using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.Common
{
  public sealed class OneFrameSystem<T> : IProtoInitSystem, IProtoRunSystem where T : struct
  {
    private readonly ProtoPool<T> _pool;
    private IProtoIt _it;

    public OneFrameSystem(ProtoPool<T> pool)
    {
      _pool = pool;
    }
    
    public void Init(IProtoSystems systems)
    {
      _it = Entities.ProtoIt<T>(systems.World());
    }

    public void Run()
    {
      foreach (var entity in _it)
        _pool.Del(entity);
    }
  }
}