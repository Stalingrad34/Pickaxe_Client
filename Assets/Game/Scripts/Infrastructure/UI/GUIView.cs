using Game.Scripts.Infrastructure.Extensions;
using UniRx;

namespace Game.Scripts.UI
{
    public abstract class GUIView<T> : GUIViewBase where T : GUIModel
    {
        protected abstract void SetModel(T model);

        protected virtual void OnClose()
        { }

        public void Init(T model)
        {
            model.Close.Subscribe(OnCloseHandler).AddTo(gameObject);
            SetModel(model);
            gameObject.DisposeModel(model);
        }

        private void OnCloseHandler(Unit _)
        {
            OnClose();
            Destroy(gameObject);
        }
    }
}