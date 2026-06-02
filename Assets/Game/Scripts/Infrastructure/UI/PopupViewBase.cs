using PrimeTween;
using UnityEngine;

namespace Game.Scripts.UI
{
  public abstract class PopupViewBase : MonoBehaviour
  {
    public CanvasGroup canvasGroup;
    public CanvasGroup backFadeGroup;
    public RectTransform panel;
    protected CanvasHolder CanvasHolder;
    
    public void SetInputActive(bool isActive)
    {
      CanvasHolder.SetActiveRaycaster(isActive);
    }
    
    public virtual Sequence GetShowTween()
    {
      if (canvasGroup != null)
        canvasGroup.alpha = 0.4f;
      
      panel.localScale = Vector3.one * 0.5f;

      var sequence = Sequence.Create(sequenceEase: Ease.Linear)
        .Chain(Tween.Scale(panel, 1.07f, 0.3f, Ease.Linear));
      
      sequence = GroupCanvasGroupTween(sequence, canvasGroup, 1, 0.3f);
      sequence = GroupCanvasGroupTween(sequence, backFadeGroup, 1, 0.3f);
      sequence = sequence.Chain(Tween.Scale(panel, 1f, 0.2f, Ease.Linear));

      return sequence;
    }

    public virtual Sequence GetHideTween()
    {
      var sequence = Sequence.Create(sequenceEase: Ease.Linear)
        .Chain(Tween.Scale(panel, 1.07f, 0.2f, Ease.Linear))
        .Chain(Tween.Scale(panel, 0.5f, 0.4f, Ease.Linear));
      
      sequence = GroupCanvasGroupTween(sequence, canvasGroup, 0, 0.2f);
      sequence = GroupCanvasGroupTween(sequence, backFadeGroup, 0, 0.2f);

      return sequence;
    }

    private static Sequence GroupCanvasGroupTween(Sequence sequence, CanvasGroup target, float value, float duration)
    {
      if (target == null)
        return sequence;

      return sequence.Group(Tween.Alpha(target, value, duration, Ease.Linear));
    }

    public void Destroy()
    {
      Destroy(CanvasHolder.gameObject);
    }
  }
}
