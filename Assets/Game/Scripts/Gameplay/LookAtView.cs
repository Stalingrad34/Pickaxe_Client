using UnityEngine;

namespace Game.Scripts.Gameplay
{
  public class LookAtView : MonoBehaviour
  {
    [SerializeField] private Vector3 worldUp = Vector3.up;
    private Camera _camera;

    private void Awake()
    {
      _camera = Camera.main;
    }

    private void LateUpdate()
    {
      var distance = transform.position - _camera.transform.position;
      transform.LookAt(transform.position + distance, worldUp);
    }
  }
}