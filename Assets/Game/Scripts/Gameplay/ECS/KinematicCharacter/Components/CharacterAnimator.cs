using UnityEngine;

namespace Game.Scripts.Gameplay.ECS.KinematicCharacter.Components
{
  public class CharacterAnimator : MonoBehaviour
  {
    private static readonly int Fly = Animator.StringToHash("Fly");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int MoveVertical = Animator.StringToHash("Move Vertical");
    private static readonly int MoveHorizontal = Animator.StringToHash("Move Horizontal");

    [SerializeField] private Animator footAnimator;

    [Header("Smoothing")]
    [Tooltip("Damping time (seconds) for Blend Tree parameters Move Horizontal / Move Vertical.")]
    [SerializeField] private float moveBlendDampTime = 0.12f;

    public void SetMove(Vector2 direction)
    {
      if (footAnimator == null)
        return;

      // Use Animator damping to make transitions between directions smooth.
      // This prevents sharp snapping when switching left/right/back/forward.
      footAnimator.SetFloat(MoveVertical, direction.y, moveBlendDampTime, Time.deltaTime);
      footAnimator.SetFloat(MoveHorizontal, direction.x, moveBlendDampTime, Time.deltaTime);
    }

    public void SetFlyAnimation(bool fly)
    {
      if (footAnimator == null)
        return;

      footAnimator.SetBool(Fly, fly);
    }

    public void SetMoveAnimation(bool move)
    {
      if (footAnimator == null)
        return;

      footAnimator.SetBool(Move, move);
    }

    public void SetAnimatorActive(bool active)
    {
      if (footAnimator == null)
        return;

      footAnimator.enabled = active;
    }

    /// <summary>
    /// Optional: allow changing damping at runtime from systems/debug UI.
    /// </summary>
    public void SetMoveBlendDampTime(float seconds)
    {
      moveBlendDampTime = Mathf.Max(0f, seconds);
    }
  }
}
