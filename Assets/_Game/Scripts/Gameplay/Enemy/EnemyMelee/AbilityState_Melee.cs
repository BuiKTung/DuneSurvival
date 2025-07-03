using _Game.Scripts.Utilities;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy.StateMachine
{
    public class AbilityState_Melee : EnemyState
    {
        private Enemy_Melee enemy;
        private Vector3 movementDirection;
        private float moveSpeed;
        private float lastTimeUse;
    
        private const float MAX_MOVEMENT_DISTANCE = 20;
        public AbilityState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
        {
            enemy = enemyBase as Enemy_Melee;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            enemy.agent.isStopped = true;
            
            enemy.EnableWeaponModel(true);
            
            moveSpeed = enemy.moveSpeed;
            movementDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
        }

        public override void OnExecute()
        {
            base.OnExecute();
        
            if (enemy.ManualRotation())
            {
                enemy.FaceTarget(enemy.player.transform.position);
                movementDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
            }

            if (enemy.ManualMovement())
            {
                enemy.transform.position =
                    Vector3.MoveTowards(enemy.transform.position, movementDirection, enemy.moveSpeed * Time.deltaTime);
            }

            if (triggerCalled)
                stateMachine.ChangeState(enemy.recoveryState);
        }
        public override void Exit()
        {
            base.Exit();
        
            enemy.moveSpeed = moveSpeed;
            enemy.anim.SetFloat(ConstantString.AnimationParameter.RecoveryIndex, 0);
        }

        public override void AbilityTrigger()
        {
            base.AbilityTrigger();

            enemy.ThrowAxe();
        }
    }
}
