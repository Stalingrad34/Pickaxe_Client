using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.GUI
{
  public class TextAnimationView : MonoBehaviour
  {
    [SerializeField] private RectTransform pickupTextArea;
    [SerializeField] private RectTransform pickupTextTarget;
    [SerializeField] private TextMeshProUGUI pickupTextView;
    [SerializeField] private AnimationCurve pickupTextAlphaCurve;
    [SerializeField] private float pickupTextDuration;
    
    public void PickupTextHandler(PickupTextData data)
    {
      var view = Instantiate(pickupTextView, pickupTextArea.transform);
      view.rectTransform.anchoredPosition = GetPickupTextPosition();
      view.text = data.Text;
      view.color = data.Color;
      view.gameObject.SetActive(true);
      
      var sequence = DOTween.Sequence();
      sequence
        .Join(GetPickupTextMoveTween(view.rectTransform))
        .Join(GetPickupTextAlphaTween(view, data.Color))
        .OnComplete(() => Destroy(view.gameObject))
        .SetLink(gameObject);
    }

    private Tween GetPickupTextMoveTween(RectTransform rectTransform)
    {
      return rectTransform.DOAnchorPos(pickupTextTarget.anchoredPosition, pickupTextDuration).SetEase(Ease.InBack);
    }
    
    private Tween GetPickupTextAlphaTween(TextMeshProUGUI text, Color color)
    {
      color.a = 0;
      return text.DOColor(color, pickupTextDuration).SetEase(pickupTextAlphaCurve);
    }

    private Vector2 GetPickupTextPosition()
    {
      var halfWidth = pickupTextArea.rect.width / 2;
      var halfHeight = pickupTextArea.rect.height / 2;
      
      var x = Random.Range(-halfWidth, halfWidth);
      var y = Random.Range(-halfHeight, halfHeight);
      
      return new Vector2(x, y);
    }
  }
}