using System.Collections.Generic;

namespace Game.Scripts.Tutorial.StartingTutorial
{
  public class StartingTutorial : AbstractTutorial
  {
    public override TutorialType Type => TutorialType.StartingTutorial;

    protected override bool CanStartInternal()
    {
      return true;
    }

    protected override List<ITutorialStep> GetSteps()
    {
      return new List<ITutorialStep>()
      {
        new BuyFirstPickaxeStep(),
        new CollectOresStep(),
        new DepositOresStep(),
        new CollectMoneyStep(),
        new BuyMorePickaxesStep(),
        new MergePickaxesStep(),
      };
    }
  }
}