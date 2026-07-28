namespace Game.Scripts.Infrastructure.States
{
    public interface IEnterStateArgs<in T> : IState
    {
        void Enter(T args);
    }
}