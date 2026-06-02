using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS
{
  public static class ECSExtensions
  {
    public static T GetAspect<T>(this ProtoWorld world) where T : class, IProtoAspect
    {
      return world.Aspect(typeof(T)) as T;
    }

    public static T GetAspect<T>(this IProtoSystems systems) where T : class, IProtoAspect
    {
      return systems.World().Aspect(typeof(T)) as T;
    }

    public static T GetService<T>(this IProtoSystems systems) where T : class
    {
      return systems.Services()[typeof(T)] as T;
    }
  }

  public static class Entities
  {
    public static ProtoIt ProtoIt<T>(ProtoWorld world) where T : struct
    {
      var result = new ProtoIt(new[] {typeof(T)});
      result.Init(world);
      return result;
    }
    
    public static ProtoIt ProtoIt<T, TU>(ProtoWorld world) where T : struct
    {
      var result = new ProtoIt(new []{typeof(T), typeof(TU)});
      result.Init(world);
      return result;
    }
    
    public static ProtoIt ProtoIt<T, TU, TV>(ProtoWorld world) where T : struct
    {
      var result = new ProtoIt(new []{typeof(T), typeof(TU), typeof(TV)});
      result.Init(world);
      return result;
    }
  }
}