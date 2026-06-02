using UnityEngine;

namespace Game.Scripts.KinematicCharacterController.Core
{
  public struct PlayerCharacterInputs
  {
    public float MoveAxisForward;
    public float MoveAxisRight;
    public Quaternion CameraRotation;
    public bool JumpDown;
    public bool CrouchDown;
    public bool CrouchUp;
  }
}