using BitGames.Bits;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure.Services;
using Game.Scripts.Infrastructure.Services.Config;
using Game.Scripts.Infrastructure.Services.Database;
using Game.Scripts.Infrastructure.States;
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
            
            /*ServiceProvider.Register(new LocalizationService());
            ServiceProvider.Register(multiplayerManager);
            ServiceProvider.Register(new DatabaseService(connectConfig));
            ServiceProvider.Register(new RatingService(connectConfig));
            ServiceProvider.Register(new AnalyticsService());
            ServiceProvider.Register(new InAppService());
            ServiceProvider.Register(new ConfigProvider(connectConfig));*/
            ServiceProvider.Register(new SettingsProvider());
            
            await ServiceProvider.InitServices();
            
            AudioController.Instance.Init();
            
            StateMachine.Init();
            StateMachine.EnterAsync<GameState>().Forget();
        }
    }
}