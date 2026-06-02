using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Multiplayer;
using Newtonsoft.Json;

namespace Game.Scripts.Infrastructure.Services.Config
{
  public class ConfigProvider : IInitializableService
  {
    public GameConfig Game { get; private set; }
    public Dictionary<string, WeaponStats> Weapons { get; private set; }
    
    private readonly string _gameConfigPath;
    private readonly string _weaponsConfigPath;
    
    public ConfigProvider(ConnectConfig connectConfig)
    {
      _gameConfigPath = connectConfig.GameConfigUrl;
      _weaponsConfigPath = connectConfig.WeaponsConfigUrl;
    }
    
    public async UniTask Init(CancellationToken token)
    {
      var tasks = new List<UniTask>();
      
      tasks.Add(LoadGameConfig(token));
      tasks.Add(LoadWeaponsConfig(token));
      
      await UniTask.WhenAll(tasks).AttachExternalCancellation(token);
    }

    private async UniTask LoadGameConfig(CancellationToken token)
    {
      var result = await HttpService.Get(_gameConfigPath);
      var versions = JsonConvert.DeserializeObject<Dictionary<string, GameConfig>>(result);
      Game = versions["v1"];
    }
    
    private async UniTask LoadWeaponsConfig(CancellationToken token)
    {
      var result = await HttpService.Get(_weaponsConfigPath);
      Weapons = JsonConvert.DeserializeObject<Dictionary<string, WeaponStats>>(result);
    }
  }
}