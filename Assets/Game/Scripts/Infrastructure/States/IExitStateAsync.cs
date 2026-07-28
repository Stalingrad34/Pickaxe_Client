using Cysharp.Threading.Tasks;

namespace Game.Scripts.Infrastructure.States
{
  public interface IExitStateAsync
  {
    UniTask ExitAsync();
  }
}