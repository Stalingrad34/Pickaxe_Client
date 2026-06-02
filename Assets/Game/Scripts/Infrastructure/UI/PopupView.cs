namespace Game.Scripts.UI
{
    public abstract class PopupView<T>: PopupViewBase where T: PopupModel
    {
        protected T Model;

        public void Init(T model, CanvasHolder canvasHolder)
        {
            CanvasHolder = canvasHolder;
            this.Model = model;
            SetModel(model);
        }
        
        protected abstract void SetModel(T model);
    }
}