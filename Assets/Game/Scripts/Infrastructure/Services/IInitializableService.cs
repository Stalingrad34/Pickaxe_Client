using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Scripts.Infrastructure.Services
{
  public interface IInitializableService : IService
  {
    UniTask Init(CancellationToken token);
  }
}