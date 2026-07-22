using Game.Scripts.Gameplay.ECS.Pickup.Interfaces;
using Game.Scripts.KinematicCharacterController.ExampleCharacter.Scripts;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Gameplay.Units
{
  public abstract class UnitView : MonoBehaviour, IPickupCollector
  {
    [SerializeField] private Animator animator;
    [SerializeField] protected Transform pickupRoot;
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected Canvas canvas;

    private ExampleCharacterCamera _camera;
    private IPickupItem _item;
    
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

    public bool CanTake()
    {
      return _item == null;
    }

    public void Take(IPickupItem item)
    {
      _item = item;
      animator.SetBool("Take", true);
      item.Transform.SetParent(pickupRoot);
      item.Transform.localPosition = Vector3.zero;
      item.Pickup();
    }

    public void Discard()
    {
      Destroy(_item.Transform.gameObject);
      _item = null;
      animator.SetBool("Take", false);
    }
  }
}