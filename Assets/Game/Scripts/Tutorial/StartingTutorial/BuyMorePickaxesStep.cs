using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay.FloorButtons;
using Game.Scripts.Gameplay.Units;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.UI.GUI;
using UniRx;

namespace Game.Scripts.Tutorial.StartingTutorial
{
  public class BuyMorePickaxesStep : ITutorialStep
  {
    private const int COMPLETE_PICKAXES_COUNT = 3;
    private MainGUIView _mainGUIView;
    private PlayerView _playerView;
    private AddPickaxeFloorButton _button;
    private TutorialTargetsContainer _targetsContainer;
    private readonly List<IDisposable> _disposables = new ();

    public async UniTask Run(TutorialTargetsContainer targetsContainer, CancellationToken token)
    {
      _targetsContainer = targetsContainer;

      var tasks = new List<UniTask>
      {
        UniTask.WaitUntil(targetsContainer.Has<MainGUIView>, cancellationToken: token),
        UniTask.WaitUntil(targetsContainer.Has<PlayerView>, cancellationToken: token),
        UniTask.WaitUntil(targetsContainer.Has<AddPickaxeFloorButton>, cancellationToken: token)
      };

      var isCanceled = await UniTask.WhenAll(tasks).SuppressCancellationThrow();
      if (isCanceled || token.IsCancellationRequested)
        return;
      
      _mainGUIView = targetsContainer.Get<MainGUIView>();
      _playerView = targetsContainer.Get<PlayerView>();
      _button = targetsContainer.Get<AddPickaxeFloorButton>();
      var pickaxesService = ServiceProvider.Get<PickaxesService>();
      var economyService = ServiceProvider.Get<EconomyService>();
      
      pickaxesService.PickaxesNominal.Subscribe(PickaxesCountChanged).AddTo(_disposables);
      economyService.Money.Subscribe(MoneyCountChanged).AddTo(_disposables);

      await UniTask.WaitUntil(() => pickaxesService.PickaxesNominal.Value >= COMPLETE_PICKAXES_COUNT, cancellationToken: token).SuppressCancellationThrow();
    }

    public void Stop()
    {
      _targetsContainer.Get<MainGUIView>()?.HideTutorialText();
      _targetsContainer.Get<PlayerView>()?.HideTutorialArrow();
      
      foreach (var disposable in _disposables)
        disposable.Dispose();
    }

    private void PickaxesCountChanged(ulong count)
    {
      var countText = $"[{count}/{COMPLETE_PICKAXES_COUNT}]";
      _mainGUIView.ShowTutorialText(new TextData("starting_tutorial_5", countText));
    }
    
    private void MoneyCountChanged(ulong count)
    {
      var pickaxeCost = ServiceProvider.Get<PickaxesService>().GetPickaxeCost();
      if (count >= pickaxeCost)
        _playerView.ShowTutorialArrow(_button.transform);
      else
        _playerView.HideTutorialArrow();
    }
  }
}