using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay.ECS;
using Game.Scripts.Gameplay.Units;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.States;
using Game.Scripts.Infrastructure.UI;
using Game.Scripts.UI.GUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.States
{
  public class GameState : IEnterStateAsync, IExitState
  {
    private MainGUIModel _mainGUIModel;
    
    public async UniTask Enter()
    {
      await SceneManager.LoadSceneAsync("Game/Scenes/Game");
      UIManager.SetCameraStack(Camera.main);
      
      _mainGUIModel = new MainGUIModel(ServiceProvider.Get<EconomyService>(), ServiceProvider.Get<PickaxesService>());
      UIManager.ShowGUI<MainGUIView, MainGUIModel>(_mainGUIModel);
      
      ECSRunner.EcsEventWriter.CreateGameSession(this, _mainGUIModel);
      
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
      ServiceProvider.Get<AdsService>().StartAdsTimer();
      ServiceProvider.Get<TutorialService>().StartTutorials().Forget();
    }

    public void Exit()
    {
      ServiceProvider.Get<PickaxesService>().StopPickaxeTimer();
    }
  }
}