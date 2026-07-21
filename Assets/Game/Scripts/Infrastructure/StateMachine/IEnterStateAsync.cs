using Cysharp.Threading.Tasks;

namespace Game.Scripts.Infrastructure.StateMachine
{
  public interface IEnterStateAsync : IState
  {
    UniTask Enter();
  }
}