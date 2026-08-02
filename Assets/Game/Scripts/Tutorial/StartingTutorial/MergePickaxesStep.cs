using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay.FloorButtons;
using Game.Scripts.Gameplay.Pickaxe;
using Game.Scripts.Gameplay.Units;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.UI.GUI;

namespace Game.Scripts.Tutorial.StartingTutorial
{
  public class MergePickaxesStep : ITutorialStep
  {
    private TutorialTargetsContainer _targetsContainer;

    public async UniTask Run(TutorialTargetsContainer targetsContainer, CancellationToken token)
    {
      _targetsContainer = targetsContainer;

      var tasks = new List<UniTask>
      {
        UniTask.WaitUntil(targetsContainer.Has<MainGUIView>, cancellationToken: token),
        UniTask.WaitUntil(targetsContainer.Has<PlayerView>, cancellationToken: token),
        UniTask.WaitUntil(targetsContainer.Has<MergePickaxesFloorButton>, cancellationToken: token)
      };

      var isCanceled = await UniTask.WhenAll(tasks).SuppressCancellationThrow();
      if (isCanceled || token.IsCancellationRequested)
        return;

      var gui = targetsContainer.Get<MainGUIView>();
      var player = targetsContainer.Get<PlayerView>();
      var button = targetsContainer.Get<MergePickaxesFloorButton>();
      var pickaxes = ServiceProvider.Get<PickaxesService>();

      gui.ShowTutorialText("starting_tutorial_6");
      player.ShowTutorialArrow(button.transform);

      await UniTask.WaitUntil(() => pickaxes.BestPickaxeType.Value > PickaxeType.Wood, cancellationToken: token).SuppressCancellationThrow();
    }
    
    public void Stop()
    {
      _targetsContainer.Get<MainGUIView>()?.HideTutorialText();
      _targetsContainer.Get<PlayerView>()?.HideTutorialArrow();
    }
  }
}