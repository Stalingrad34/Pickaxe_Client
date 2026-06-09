using Game.Scripts.KinematicCharacterController.ExampleCharacter.Scripts;
using UnityEngine;

namespace Game.Scripts.Gameplay.Units
{
  public class EnemyView : UnitView
  {
    protected override void Init(UnitData data, ExampleCharacterCamera playerCamera)
    {
      transform.position = data.Position;
      transform.rotation = Quaternion.Euler(0, data.StartAngleY, 0);
    }
  }
}