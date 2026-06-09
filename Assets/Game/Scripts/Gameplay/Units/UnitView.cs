using Game.Scripts.KinematicCharacterController.ExampleCharacter.Scripts;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Gameplay.Units
{
  public abstract class UnitView : MonoBehaviour
  {
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected Canvas canvas;

    private ExampleCharacterCamera _camera;
    private GameObject _weapon;
    
    public void Setup(UnitData data, ExampleCharacterCamera playerCamera)
    {
      var setupComponents = gameObject.GetComponents<IUnitSetup>();
      foreach (var setupComponent in setupComponents)
      {
        setupComponent.Setup(data);
      }

      Init(data, playerCamera);
      ChangeName(data.PlayerName);
      _camera = playerCamera;
    }

    protected abstract void Init(UnitData data, ExampleCharacterCamera playerCamera);

    public void ChangeName(string playerName)
    {
      nameText.text = playerName;
    }

    private void LateUpdate()
    {
      if (_camera?.Camera == null) return;
      
      var directionToCamera = _camera.Camera.transform.position - transform.position;
      canvas.transform.rotation = Quaternion.LookRotation(-directionToCamera);
    }
  }
}