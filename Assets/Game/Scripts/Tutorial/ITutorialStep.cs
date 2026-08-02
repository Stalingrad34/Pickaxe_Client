using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Scripts.Tutorial
{
  public interface ITutorialStep
  {
    UniTask Run(TutorialTargetsContainer targetsContainer, CancellationToken token);
    void Stop();
  }
}