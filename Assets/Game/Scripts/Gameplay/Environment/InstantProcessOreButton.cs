using Game.Scripts.Infrastructure.Services;
using UniRx;

namespace Game.Scripts.Gameplay.Environment
{
    public class InstantProcessOreButton : WorldInteractableButton
    {
        public override void Awake()
        { 
            base.Awake();
            ServiceProvider.Get<EconomyService>().ProcessingOre.Subscribe(ProcessingOreChanged).AddTo(gameObject);
        }

        protected override void OnClick()
        {
            ServiceProvider.Get<OreProcessingService>().InstantProcessOreByAds();
        }

        private void ProcessingOreChanged(ulong amount)
        {
            gameObject.SetActive(amount > 0);
        }
    }
}
