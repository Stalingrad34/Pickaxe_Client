using Cysharp.Threading.Tasks;

namespace Game.Scripts.Infrastructure.StateMachine
{
  public interface IExitStateAsync
  {
    UniTask ExitAsync();
  }
}