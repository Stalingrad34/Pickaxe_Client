using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Multiplayer;
using Game.Scripts.States;
using UnityEngine;

namespace Game.Scripts.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private ConnectConfig connectConfig;
        [SerializeField] private MultiplayerManager multiplayerManager;
        
        private void Start()
        {
            RunAsync().Forget();
        }

        private async UniTaskVoid RunAsync()
        {
            Application.targetFrameRate = 60;
            Application.runInBackground = true;

            if (Platform.IsAndroid() || Platform.IsiOS())
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.orientation = ScreenOrientation.AutoRotation;
            }
            
            //multiplayerManager.Init(connectConfig);
            
            /*
            ServiceProvider.Register(multiplayerManager);
            
            ServiceProvider.Register(new RatingService(connectConfig));
            ServiceProvider.Register(new AnalyticsService());
            ServiceProvider.Register(new InAppService());
            ServiceProvider.Register(new ConfigProvider(connectConfig));*/

            var storageService = new StorageService();
            var playerService = new PlayerService();
            var economyService = new EconomyService();
            var pickaxesService = new PickaxesService();
            var localizationService = new LocalizationService();
            var oreProcessingService = new OreProcessingService(economyService);
            
            storageService
                .AddProcessor(playerService)
                .AddProcessor(economyService)
                .AddProcessor(pickaxesService)
                .AddProcessor(localizationService)
                .AddProcessor(oreProcessingService);
            
            ServiceProvider.Register(storageService);
            ServiceProvider.Register(playerService);
            ServiceProvider.Register(economyService);
            ServiceProvider.Register(pickaxesService);
            ServiceProvider.Register(localizationService);
            ServiceProvider.Register(new SettingsProvider());
            ServiceProvider.Register(oreProcessingService);
            ServiceProvider.Register(new TimeProvider());
            
            await ServiceProvider.InitServices();
            
            AudioController.Instance.Init();
            
            StateMachine.StateMachine.Init();
            StateMachine.StateMachine.EnterAsync<GameState>().Forget();
        }
    }
}