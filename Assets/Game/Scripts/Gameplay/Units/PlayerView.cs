using Game.Scripts.KinematicCharacterController.Core;
using Game.Scripts.KinematicCharacterController.ExampleCharacter.Scripts;
using UnityEngine;

namespace Game.Scripts.Gameplay.Units
{
  public class PlayerView : UnitView
  {
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private KinematicCharacterProcessor processor;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float arrowHeight;

    private Transform _arrowTarget;
    private float _arrowOffset;
    
    protected override void Init(UnitData data, ExampleCharacterCamera playerCamera)
    {
      playerCamera.SetFollowTransform(processor.CameraFollowPoint);
      processor.Motor.SetPosition(data.Position);
      processor.Motor.SetRotation(Quaternion.Euler(0, data.StartAngleY, 0));
      playerCamera.Transform.rotation = Quaternion.Euler(0, data.StartAngleY, 0);
      playerCamera.PlanarDirection = Quaternion.Euler(0, data.StartAngleY, 0) * playerCamera.PlanarDirection;
    }

    protected override void LateUpdate()
    {
      base.LateUpdate();

      if (_arrowTarget == null)
      {
        lineRenderer.enabled = false;
        return;
      }
      
      lineRenderer.SetPosition(0, transform.position + Vector3.up * arrowHeight);
      lineRenderer.SetPosition(1, _arrowTarget.position + Vector3.up * arrowHeight);
      lineRenderer.enabled = true;
      
      _arrowOffset -= Time.deltaTime * arrowSpeed;
      lineRenderer.material.mainTextureOffset = new Vector2(_arrowOffset, 0f);
    }

    public void ShowTutorialArrow(Transform target)
    {
      _arrowTarget = target;
    }

    public void HideTutorialArrow()
    {
      _arrowTarget = null;
    }
  }
}