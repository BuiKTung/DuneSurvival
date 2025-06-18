using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy
{
    public class RecoveryState_Melee : EnemyState
    {
        private Enemy_Melee enemy;
        public RecoveryState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
        {
            enemy = base.enemyBase as Enemy_Melee;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            enemy.agent.isStopped = true;
        }

        public override void OnExecute()
        {
            base.OnExecute();
            enemy.FaceTarget(enemy.player.transform.position);

            if (triggerCalled)
            {   
                if(enemy.CanThrowAxe())
                    stateMachine.ChangeState(enemy.abilityState);
                else if(enemy.PlayerInAttackRange())
                    stateMachine.ChangeState(enemy.attackState);
                else
                    stateMachine.ChangeState(enemy.chaseState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            enemy.agent.isStopped = false;
        }
    }
}
