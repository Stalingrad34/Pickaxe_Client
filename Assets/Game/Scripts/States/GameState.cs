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

      var database = ServiceProvider.Get<DatabaseService>();
      UIManager.ShowGUI<MainGUIView, MainGUIModel>(new MainGUIModel(database));
      
      var data = new UnitData()
      {
        Id = "player",
        Speed = 2,
        //PlayerSpeed = ServiceProvider.Get<ConfigProvider>().Game.Speed,
        //JumpForce = ServiceProvider.Get<ConfigProvider>().Game.JumpForce,
        PlayerName = "Player",
        StartAngleY = 0,
        Position = Vector3.back * 30,
      };
      
      ECSRunner.EcsEventWriter.SpawnCharacter(data, "Player");
    }
  }
}