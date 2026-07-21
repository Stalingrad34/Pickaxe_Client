namespace Game.Scripts.Infrastructure.UI
{
    public class PopupModel : DisposableModel
    {
        public void Close()
        {
            UIManager.HidePopup(this);
        }
    }
}