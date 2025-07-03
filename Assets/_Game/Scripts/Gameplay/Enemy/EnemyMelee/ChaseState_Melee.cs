using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy.StateMachine
{
    public class ChaseState_Melee : EnemyState
    {
        public Enemy_Melee enemy;

        [SerializeField] private float lastTimeUpdateDestanation;
        public ChaseState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
        {
            enemy = enemyBase as Enemy_Melee;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            enemy.agent.speed = enemy.chaseSpeed;
        }

        public override void OnExecute()
        {
            base.OnExecute();
            
            if(enemy.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.attackState);
            
            enemy.FaceTarget(GetNextPathPoint());
            
            if (CanUpdateDestination())
            {
                enemy.agent.destination = enemy.player.transform.position;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        private bool CanUpdateDestination()
        {
            if (Time.time > lastTimeUpdateDestanation + .25f)
            {
                lastTimeUpdateDestanation = Time.time;
                return true;
            }
            return false;
        }
    }
}
