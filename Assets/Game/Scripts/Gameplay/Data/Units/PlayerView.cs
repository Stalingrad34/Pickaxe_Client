using Game.Scripts.KinematicCharacterController.Core;
using Game.Scripts.KinematicCharacterController.ExampleCharacter.Scripts;
using UnityEngine;

namespace Game.Scripts.Gameplay.Data.Units
{
  public class PlayerView : UnitView
  {
    [SerializeField] private KinematicCharacterProcessor processor;
    
    protected override void Init(UnitData data, ExampleCharacterCamera playerCamera)
    {
      playerCamera.SetFollowTransform(processor.CameraFollowPoint);
      processor.Motor.SetPosition(data.Position);
      processor.Motor.SetRotation(Quaternion.Euler(0, data.StartAngleY, 0));
      playerCamera.Transform.rotation = Quaternion.Euler(0, data.StartAngleY, 0);
      playerCamera.PlanarDirection = Quaternion.Euler(0, data.StartAngleY, 0) * playerCamera.PlanarDirection;
    }
  }
}