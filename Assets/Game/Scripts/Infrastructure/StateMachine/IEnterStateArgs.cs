namespace Game.Scripts.Infrastructure.StateMachine
{
    public interface IEnterStateArgs<in T> : IState
    {
        void Enter(T args);
    }
}