using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.Config;
using Game.Scripts.Infrastructure.Services.InApp;
using Game.Scripts.Infrastructure.Services.Storage;
using Game.Scripts.Infrastructure.Sound;
using Game.Scripts.Infrastructure.States;
using Game.Scripts.Multiplayer;
using Game.Scripts.States;
using UnityEngine;
using TimeProvider = Game.Scripts.Infrastructure.Services.TimeProvider;

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

            var storageService = new StorageService();
            var economyService = new EconomyService();
            var pickaxesService = new PickaxesService();
            var localizationService = new LocalizationService();
            var oreProcessingService = new OreProcessingService(economyService, pickaxesService);
            var tutorialService = new TutorialService();
            var adsService = new AdsService();
            var playerService = new PlayerService();
            var ratingService = new RatingService(playerService);
            var reviewService = new ReviewService(pickaxesService);
            
            storageService
                .AddProcessor(economyService)
                .AddProcessor(pickaxesService)
                .AddProcessor(tutorialService)
                .AddProcessor(reviewService)
                .AddProcessor(adsService);
            
            ServiceProvider.Register(storageService);
            ServiceProvider.Register(playerService);
            ServiceProvider.Register(economyService);
            ServiceProvider.Register(pickaxesService);
            ServiceProvider.Register(localizationService);
            ServiceProvider.Register(oreProcessingService);
            ServiceProvider.Register(tutorialService);
            ServiceProvider.Register(ratingService);
            ServiceProvider.Register(adsService);
            ServiceProvider.Register(reviewService);
            ServiceProvider.Register(new SettingsProvider());
            ServiceProvider.Register(new ChestService());
            ServiceProvider.Register(new TimeProvider());
            ServiceProvider.Register(new InAppService());
            ServiceProvider.Register(new AnalyticsService());
            ServiceProvider.Register(new ConfigProvider());

            await ServiceProvider.InitServices();
            
            AudioController.Instance.Init();
            
            StateMachine.Init();
            StateMachine.EnterAsync<GameState>().Forget();
        }
    }
}