using Game.Scripts.KinematicCharacterController.ExampleCharacter.Scripts;
using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.Camera.Components
{
  public struct CameraComponent
  {
    public ExampleCharacterCamera Camera;
    public Vector3 LookInput;
    public float ScrollInput;
  }
}