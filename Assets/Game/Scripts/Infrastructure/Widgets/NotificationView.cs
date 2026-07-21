using Game.Scripts.Infrastructure.Custom;
using PrimeTween;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Widgets
{
  public class NotificationView : MonoBehaviour
  {
    [SerializeField] private CustomText notificationText;
    private Sequence _sequence;
    
    public void Show(string text)
    {
      notificationText.SetText(text);
      _sequence.Complete();

      transform.localScale = Vector3.zero;
      gameObject.SetActive(true);

      _sequence = Sequence.Create(sequenceEase: Ease.Linear)
        .Chain(Tween.Scale(transform, Vector3.one, 0.1f, Ease.Linear))
        .ChainDelay(1.5f)
        .ChainCallback(() => gameObject.SetActive(false));
    }

    public void Hide()
    {
      _sequence.Complete();
    }
  }
}
