using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay.ECS;
using Game.Scripts.Gameplay.Units;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.Config;
using Game.Scripts.Infrastructure.StateMachine;
using Game.Scripts.Infrastructure.UI;
using Game.Scripts.UI;
using Game.Scripts.UI.GUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.States
{
  public class GameState : IEnterStateAsync, IExitState
  {
    public async UniTask Enter()
    {
      await SceneManager.LoadSceneAsync("Game/Scenes/Game");
      UIManager.SetCameraStack(Camera.main);
      
      var model = new MainGUIModel(ServiceProvider.Get<EconomyService>(), ServiceProvider.Get<PickaxesService>());
      UIManager.ShowGUI<MainGUIView, MainGUIModel>(model);
      
      ECSRunner.EcsEventWriter.CreateGameSession(this, model);
      
      var data = new UnitData()
      {
        Id = "player",
        PlayerName = "Player"
      };
      
      ECSRunner.EcsEventWriter.SpawnCharacter(data, "Player");
      
      var pickaxeService = ServiceProvider.Get<PickaxesService>();
      pickaxeService.RebuildPickaxes("player");
      pickaxeService.StartPickaxeTimer();
      pickaxeService.CheckRestOre();
      
      ServiceProvider.Get<OreProcessingService>().StartTimers();
    }

    public void Exit()
    {
      ServiceProvider.Get<PickaxesService>().StopPickaxeTimer();
    }
  }
}