using UniRx;

namespace Game.Scripts.UI
{
    public abstract class GUIModel : DisposableModel
    {
        public readonly ReactiveCommand Close = new ();
    }
}