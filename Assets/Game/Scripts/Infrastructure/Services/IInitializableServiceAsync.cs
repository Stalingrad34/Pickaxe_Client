using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Scripts.Infrastructure.Services
{
  public interface IInitializableServiceAsync : IService
  {
    UniTask Init(CancellationToken token);
  }
}