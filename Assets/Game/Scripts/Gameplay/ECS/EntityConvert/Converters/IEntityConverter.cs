using Leopotam.EcsProto;

namespace Game.Scripts.Gameplay.ECS.EntityConvert.Converters
{
  public interface IEntityConverter
  {
    void Convert(ProtoEntity entity, IProtoSystems systems);
  }
}