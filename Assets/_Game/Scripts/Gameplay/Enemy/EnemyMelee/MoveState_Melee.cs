using _Game.Scripts.Gameplay.Enemy.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace _Game.Scripts.Gameplay.Enemy
{
    public class MoveState_Melee : EnemyState
    {
        private Enemy_Melee enemy;
        private Vector3 destination;
        public MoveState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
        {
            enemy = enemyBase as Enemy_Melee;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            enemy.agent.speed = enemy.moveSpeed;
            destination = enemy.GetPatrolDestination();
            enemy.agent.SetDestination(destination);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void OnExecute()
        {
            base.OnExecute();
            
            enemy.FaceTarget(GetNextPathPoint());
            
            if(enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + .05f)
                stateMachine.ChangeState(enemy.idleState);
            
        }

        
    }
}
