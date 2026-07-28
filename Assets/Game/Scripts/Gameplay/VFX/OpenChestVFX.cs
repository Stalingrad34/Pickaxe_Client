using DG.Tweening;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Infrastructure.Custom;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Gameplay.VFX
{
  public class OpenChestVFX : MonoBehaviour
  {
    [SerializeField] private CanvasGroup root;
    [SerializeField] private Image icon;
    [SerializeField] private CustomText pickaxeName;
    [SerializeField] private CustomText pickaxeTier;
    [SerializeField] private float duration;
    [SerializeField] private float delay;
    [SerializeField] private Ease ease;
    
    private Sequence _sequence;

    public void ShowPickaxe(PickaxeConfig pickaxeConfig)
    {
      icon.sprite = pickaxeConfig.available;
      pickaxeName.SetText(pickaxeConfig.nameKey);
      var level = (int) pickaxeConfig.pickaxeType;
      pickaxeTier.SetText(new TextData("pickaxe_level", level.ToString()));
      
      _sequence?.Complete();
      _sequence = DOTween.Sequence();
      _sequence.OnStart(() =>
      {
        root.transform.localScale = Vector3.zero;
        root.alpha = 0;
      });

      _sequence
        .Append(GetShowTween())
        .AppendInterval(delay)
        .Append(GetHideTween());
    }

    private Tween GetShowTween()
    {
      var sequence = DOTween.Sequence();
      sequence
        .Append(root.transform.DOScale(Vector3.one, duration))
        .Join(root.DOFade(1, duration))
        .SetEase(ease);

      return sequence;
    }
    
    private Tween GetHideTween()
    {
      var sequence = DOTween.Sequence();
      sequence
        .Append(root.transform.DOScale(Vector3.zero, duration))
        .Join(root.DOFade(0, duration))
        .SetEase(ease);

      return sequence;
    }
  }
}