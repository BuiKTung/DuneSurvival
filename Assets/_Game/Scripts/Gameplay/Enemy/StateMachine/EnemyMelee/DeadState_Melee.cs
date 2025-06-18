using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy.StateMachine
{
    public class DeadState_Melee : EnemyState
    {
        public Enemy_Melee enemy;
        private Enemy_Ragdoll ragdoll;
        private bool interactionDisabled;
        public DeadState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
        {
            enemy = base.enemyBase as Enemy_Melee;
            ragdoll = enemy.GetComponent<Enemy_Ragdoll>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            enemy.anim.enabled = false;
            enemy.agent.isStopped = true;
            enemy.agent.ResetPath();
            
            interactionDisabled = false;

            stateTimer = 5.5f;
            
            ragdoll.RagdollActive(true);
        }

        public override void OnExecute()
        {
            base.OnExecute();

            if (stateTimer < 0 && interactionDisabled == false)
            {
                interactionDisabled = true;
                ragdoll.RagdollActive(false);
                ragdoll.ColliderActive(false);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
