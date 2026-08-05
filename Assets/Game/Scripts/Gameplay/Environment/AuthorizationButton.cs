using Game.Scripts.Infrastructure.Services;

namespace Game.Scripts.Gameplay.Environment
{
  public class AuthorizationButton : WorldInteractableButton
  {
    protected override void OnClick()
    {
      ServiceProvider.Get<PlayerService>().OpenAuthDialogue();
    }
  }
}