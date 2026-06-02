namespace Game.Scripts.UI
{
    public class PopupModel : DisposableModel
    {
        public void Close()
        {
            UIManager.HidePopup(this);
        }
    }
}