using Game.Scripts.Gameplay.ECS.EntityConvert.Converters;
using Game.Scripts.Gameplay.Ore;
using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Pickup.Converters
{
  public class PickupOreItemConverter : MonoBehaviour, IEntityConverter, IOreSetup
  {
    private ulong _amount;
    private Color _pickupColor;
    
    public void Convert(ProtoEntity entity, IProtoSystems systems)
    {
      var aspect = systems.GetAspect<PickupAspect>();
      ref var component = ref aspect.PickupOreItemsPool.Add(entity);
      component.Amount = _amount;
      component.PickupTextColor = _pickupColor;
    }

    public void Setup(OreData data)
    {
      _amount = data.Amount;
      _pickupColor = data.PickupTextColor;
    }
  }
}