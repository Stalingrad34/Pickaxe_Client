using Cysharp.Threading.Tasks;

namespace Game.Scripts.Infrastructure.StateMachine
{
  public interface IEnterStateArgsAsync<in T> : IState
  {
    UniTask Enter(T args);
  }
}