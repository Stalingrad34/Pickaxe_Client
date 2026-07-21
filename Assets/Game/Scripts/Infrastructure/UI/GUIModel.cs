using UniRx;

namespace Game.Scripts.Infrastructure.UI
{
    public abstract class GUIModel : DisposableModel
    {
        public readonly ReactiveCommand Close = new ();
    }
}