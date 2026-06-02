namespace Game.Scripts.Gameplay.ECS.Input.Components
{
  public struct ControlComponent
  {
    public bool IsKeysLocked;
    public bool IsTouchLocked;
    public float HorizontalAxis;
    public float VerticalAxis;
    public float MouseHorizontal;
    public float MouseVertical;
    public float MouseScroll;
    public bool MouseLeftClicked;
    public bool SpaceKeyDown;
  }
}