namespace _Game.Scripts.Gameplay.Enemy.StateMachine
{
    public interface IState 
    {
        void OnEnter();
        void OnExecute();
        void Exit();
    }
}