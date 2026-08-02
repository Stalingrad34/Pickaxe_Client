using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay.FloorButtons;
using Game.Scripts.Gameplay.Units;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.UI.GUI;

namespace Game.Scripts.Tutorial.StartingTutorial
{
  public class CollectMoneyStep : ITutorialStep
  {
    private TutorialTargetsContainer _targetsContainer;

    public async UniTask Run(TutorialTargetsContainer targetsContainer, CancellationToken token)
    {
      _targetsContainer = targetsContainer;

      var tasks = new List<UniTask>
      {
        UniTask.WaitUntil(targetsContainer.Has<MainGUIView>, cancellationToken: token),
        UniTask.WaitUntil(targetsContainer.Has<PlayerView>, cancellationToken: token),
        UniTask.WaitUntil(targetsContainer.Has<CollectMoneyFloorButton>, cancellationToken: token)
      };

      var isCanceled = await UniTask.WhenAll(tasks).SuppressCancellationThrow();
      if (isCanceled || token.IsCancellationRequested)
        return;

      var gui = targetsContainer.Get<MainGUIView>();
      var player = targetsContainer.Get<PlayerView>();
      var button = targetsContainer.Get<CollectMoneyFloorButton>();
      var economy = ServiceProvider.Get<EconomyService>();

      gui.ShowTutorialText("starting_tutorial_4");
      player.ShowTutorialArrow(button.transform);

      var nextPickaxeCost = ServiceProvider.Get<PickaxesService>().GetPickaxeCost();
      await UniTask.WaitUntil(() => economy.Money.Value >= nextPickaxeCost, cancellationToken: token).SuppressCancellationThrow();
    }
    
    public void Stop()
    {
      _targetsContainer.Get<MainGUIView>()?.HideTutorialText();
      _targetsContainer.Get<PlayerView>()?.HideTutorialArrow();
    }
  }
}