using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay.Units;
using Game.Scripts.Infrastructure.Custom;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.UI.GUI;
using UniRx;

namespace Game.Scripts.Tutorial.StartingTutorial
{
  public class CollectOresStep : ITutorialStep
  {
    private const int COLLECT_ORE_COUNT = 3;
    private MainGUIView _mainGUIView;
    private IDisposable _subscribe;
    private TutorialTargetsContainer _targetsContainer;

    public async UniTask Run(TutorialTargetsContainer targetsContainer, CancellationToken token)
    {
      _targetsContainer = targetsContainer;
      
      var tasks = new List<UniTask>
      {
        UniTask.WaitUntil(targetsContainer.Has<MainGUIView>, cancellationToken: token),
        UniTask.WaitUntil(targetsContainer.Has<PlayerView>, cancellationToken: token),
        UniTask.WaitUntil(targetsContainer.Has<CollectOresTutorialTarget>, cancellationToken: token)
      };

      var isCanceled = await UniTask.WhenAll(tasks).SuppressCancellationThrow();
      if (isCanceled || token.IsCancellationRequested)
        return;
      
      _mainGUIView = targetsContainer.Get<MainGUIView>();
      var player = targetsContainer.Get<PlayerView>();
      var collectOresTarget = targetsContainer.Get<CollectOresTutorialTarget>();
      var economy = ServiceProvider.Get<EconomyService>();
      
      _subscribe = economy.Ore.Subscribe(CollectOresChanged);
      player.ShowTutorialArrow(collectOresTarget.transform);

      await UniTask.WaitUntil(() => economy.Ore.Value >= COLLECT_ORE_COUNT, cancellationToken: token).SuppressCancellationThrow();
    }

    public void Stop()
    {
      _targetsContainer.Get<MainGUIView>()?.HideTutorialText();
      _targetsContainer.Get<PlayerView>()?.HideTutorialArrow();
      _subscribe.Dispose();
    }

    private void CollectOresChanged(ulong count)
    {
      var countText = $"[{count}/{COLLECT_ORE_COUNT}]";
      _mainGUIView.ShowTutorialText(new TextData("starting_tutorial_2", countText));
    }
  }
}