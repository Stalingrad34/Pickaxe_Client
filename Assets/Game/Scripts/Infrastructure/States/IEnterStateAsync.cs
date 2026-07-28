using Cysharp.Threading.Tasks;

namespace Game.Scripts.Infrastructure.States
{
  public interface IEnterStateAsync : IState
  {
    UniTask Enter();
  }
}