using Game.Scripts.Infrastructure.Extensions;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Scripts.UI.GUI
{
  public class MainGUIView : GUIView<MainGUIModel>
  {
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject joystick;
    
    protected override void SetModel(MainGUIModel model)
    {
      model.Money.SubscribeMoney(moneyText).AddTo(gameObject);
      model.ShowJoystick.Subscribe(joystick.SetActive).AddTo(gameObject);
    }
  }
}