using System.Collections.Generic;
using _Game.Scripts.Utilities;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy.StateMachine
{
    public class AttackState_Melee : EnemyState
    {
        private Enemy_Melee enemy;
        private float attackMoveSpeed;
        private Vector3 attackDirection;

        private const float MAX_ATTACK_DISTANCE = 50f;
    
        public AttackState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
        {
            enemy = enemyBase as Enemy_Melee;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            SetUpAttack();
        }

        private void SetUpAttack()
        {
            enemy.EnableWeaponModel(true);
            enemy.enemyVisuals.EnableWeaponTrail(true);
            
            enemy.agent.isStopped = true;
            enemy.agent.velocity = Vector3.zero;

            attackMoveSpeed = enemy.attackDataEnemyMelee.moveSpeed;
            enemy.anim.SetFloat(ConstantString.AnimationParameter.AttackAnimationSpeed, enemy.attackDataEnemyMelee.animationSpeed);
            enemy.anim.SetFloat(ConstantString.AnimationParameter.AttackIndex, enemy.attackDataEnemyMelee.attackIndex);
            enemy.anim.SetFloat(ConstantString.AnimationParameter.SlashAttackIndex, Random.Range(0, ConstantString.AnimationParameter.SlashAttackCount));
            
            attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
        }

        public override void OnExecute()
        {
            base.OnExecute();

            if (enemy.ManualRotation())
            {
                enemy.FaceTarget(enemy.player.transform.position);
                attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
            }
            
            if (enemy.ManualMovement())
            {
                enemy.transform.position =
                    Vector3.MoveTowards(enemy.transform.position, attackDirection, attackMoveSpeed * Time.deltaTime);
            }

            if(triggerCalled)
            {
                if(enemy.PlayerInAttackRange())
                    stateMachine.ChangeState(enemy.recoveryState);
                else
                    stateMachine.ChangeState(enemy.chaseState);
            }
            
                
        }
        public override void Exit()
        {
            base.Exit();
            enemy.agent.isStopped = false;
            enemy.enemyVisuals.EnableWeaponTrail(false);
            SetupNextAttack();
        }

        private void SetupNextAttack()
        {
            enemy.anim.SetFloat(ConstantString.AnimationParameter.RecoveryIndex, PlayerClose() ? 1 : 0);

            enemy.attackDataEnemyMelee = updateAttackData();
        }

        private bool PlayerClose() => Vector3.Distance(enemy.transform.position, enemy.player.transform.position) <= 1;

        private AttackData_EnemyMelee updateAttackData()
        {
            List<AttackData_EnemyMelee> validAttack = new List<AttackData_EnemyMelee>(enemy.attackList);
            
            if(PlayerClose())
                validAttack.RemoveAll(a => a.AttackType == AttackTypeMelee.Charge);
            
            int random = Random.Range(0, validAttack.Count);
            return validAttack[random];
        }
    }
}
