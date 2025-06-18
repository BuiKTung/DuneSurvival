using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy
{
    public class EnemyStateMachine 
    {
        public EnemyState currentState { get; private set; }

        public void Initialize(EnemyState startState)
        {
            currentState = startState;
            currentState.OnEnter();
        }

        public void ChangeState(EnemyState newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.OnEnter();
        }
    }
}
