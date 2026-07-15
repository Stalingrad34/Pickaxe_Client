using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Services
{
  public static class ServiceProvider
  {
    private static readonly List<IService> Container = new();
    private static readonly CancellationTokenSource CancellationToken = new();

    public static async UniTask InitServices()
    {
      var tasks = new List<UniTask>();
      foreach (var service in Container)
      {
        if (service is IInitializableService initializedService)
        {
          tasks.Add(initializedService.Init(CancellationToken.Token));
        }
      }
      
      await UniTask.WhenAll(tasks).SuppressCancellationThrow();
    }
    
    public static TService Get<TService>() where TService : IService
    {
      foreach (var service in Container)
      {
        if (service is TService result)
          return result;
      }

      throw new UnityException($"Service {nameof(TService)} not registered");
    }
    
    public static bool Has<TService>() where TService : IService
    {
      foreach (var service in Container)
      {
        if (service is TService)
          return true;
      }

      return false;
    }

    public static void Register<T>(T service) where T: IService
    {
      Container.Add(service);
    }
  }
  
}