namespace Game.Scripts.Infrastructure.Services
{
  public interface IInitializableService : IService
  {
    void Init();
  }
}