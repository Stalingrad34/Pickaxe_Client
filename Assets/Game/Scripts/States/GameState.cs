using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay.ECS;
using Game.Scripts.Gameplay.Units;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.Config;
using Game.Scripts.Infrastructure.Services.Database;
using Game.Scripts.Infrastructure.States;
using Game.Scripts.UI;
using Game.Scripts.UI.GUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.States
{
  public class GameState : IEnterStateAsync
  {
    public async UniTask Enter()
    {
      await SceneManager.LoadSceneAsync("Game/Scenes/Game");
      UIManager.SetCameraStack(Camera.main);

      var economy = ServiceProvider.Get<EconomyService>();
      UIManager.ShowGUI<MainGUIView, MainGUIModel>(new MainGUIModel(economy));
      
      var data = new UnitData()
      {
        Id = "player",
        Speed = 2,
        //PlayerSpeed = ServiceProvider.Get<ConfigProvider>().Game.Speed,
        //JumpForce = ServiceProvider.Get<ConfigProvider>().Game.JumpForce,
        PlayerName = "Player"
      };
      
      ECSRunner.EcsEventWriter.SpawnCharacter(data, "Player");
      ServiceProvider.Get<PickaxesService>().RebuildPickaxes("player");
    }
  }
}