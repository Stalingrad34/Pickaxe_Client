using DG.Tweening;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Extensions;
using Game.Scripts.Infrastructure.Services;
using UniRx;
using UnityEngine;

namespace Game.Scripts.UI.GUI
{
  public class DesktopButtonView : MonoBehaviour
  {
    [SerializeField] private CustomButton actionBtn;
    private DesktopShortcutService _desktopShortcutService;

    private void Start()
    {
      transform
        .DOScale(Vector3.one * 1.1f, 0.5f)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.Linear)
        .SetLink(gameObject);

      _desktopShortcutService = ServiceProvider.Get<DesktopShortcutService>();
      _desktopShortcutService.CanShow.Subscribe(gameObject.SetActive).AddTo(gameObject);
      actionBtn.OnClick(ShowDialog).AddTo(gameObject);
    }

    private void ShowDialog()
    {
      _desktopShortcutService.ShowDialog();
    }
  }
}