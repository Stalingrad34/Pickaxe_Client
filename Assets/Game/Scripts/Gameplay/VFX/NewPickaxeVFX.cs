using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Infrastructure.Custom;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Gameplay.VFX
{
  public class NewPickaxeVFX : MonoBehaviour
  {
    [SerializeField] private RectTransform vfx;
    [SerializeField] private Image pickaxeIcon;
    [SerializeField] private CustomText pickaxeName;
    [SerializeField] private GameObject newText;
    [SerializeField] private RectTransform startPosition;
    [SerializeField] private RectTransform showPosition;
    [SerializeField] private RectTransform finishPosition;
    [SerializeField] private float duration;
    [SerializeField] private float delay;
    [SerializeField] private Ease ease;

    private readonly Queue<PickaxeConfig> _pickaxes = new();
    private bool _isBusy;
    private Sequence _sequence;

    public void ShowVFX(PickaxeConfig pickaxeConfig)
    {
      _pickaxes.Enqueue(pickaxeConfig);

      if (!_isBusy)
        LaunchVFX().Forget();
    }

    private async UniTaskVoid LaunchVFX()
    {
      _isBusy = true;
      
      while (_pickaxes.Count > 0)
      {
        var pickaxe =  _pickaxes.Dequeue();
        await Play(pickaxe);
      }
      
      _isBusy = false;
    }

    private async UniTask Play(PickaxeConfig pickaxeConfig)
    {
      _sequence?.Kill();
      _sequence = DOTween.Sequence();
      _sequence.OnStart(() => Prepare(pickaxeConfig));
      _sequence
        .Append(GetShowTween())
        .AppendCallback(() => newText.SetActive(true))
        .AppendInterval(delay)
        .AppendCallback(() => newText.SetActive(false))
        .Append(vfx.DOAnchorPos(finishPosition.anchoredPosition, duration))
        .Join(vfx.DOScale(finishPosition.localScale, duration))
        .OnComplete(() => vfx.gameObject.SetActive(false));

      await _sequence.AsyncWaitForCompletion();
    }

    private void Prepare(PickaxeConfig pickaxeConfig)
    {
      vfx.localPosition = startPosition.localPosition;
      vfx.localRotation = startPosition.localRotation;
      vfx.localScale = Vector3.one;
      vfx.gameObject.SetActive(true);
      newText.SetActive(false);
      pickaxeIcon.sprite = pickaxeConfig.available;
      pickaxeName.SetText(pickaxeConfig.nameKey);
    }

    private Tween GetShowTween()
    {
      var sequence = DOTween.Sequence();
      
      sequence
        .Append(vfx.DOAnchorPos(showPosition.anchoredPosition, duration))
        .Join(vfx.DOLocalRotate(Vector3.zero, duration))
        .SetEase(ease);

      return sequence;
    }
  }
}