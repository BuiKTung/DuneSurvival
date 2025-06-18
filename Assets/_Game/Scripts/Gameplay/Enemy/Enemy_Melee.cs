using System;
using System.Collections.Generic;
using _Game.Scripts.Gameplay.Enemy.StateMachine;
using _Game.Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Gameplay.Enemy
{
    [System.Serializable]
    public struct AttackData_EnemyMelee
    {
        public string attackName;
        public float attackRange;
        public float moveSpeed;
        public float attackIndex;
        [Range(1,2)]
        public float animationSpeed;
        public AttackTypeMelee AttackType;
    }

    public enum AttackTypeMelee
    {
        Close,
        Charge
    }

    public enum EnemyMelee_Type
    {
        Regular,Shield, Dodge, AxeThrow
    }
    public class Enemy_Melee : Enemy
    {
        public Enemy_Visuals enemyVisuals {get; private set;}
        public IdleState_Melee idleState { get; private set; }
        public MoveState_Melee moveState { get; private set; }
        public RecoveryState_Melee recoveryState { get; private set; }
        public ChaseState_Melee chaseState { get; private set; }
        public AttackState_Melee attackState { get; private set; }
        public DeadState_Melee deadState { get; private set; }
        public AbilityState_Melee abilityState { get; private set; }
        
        [Header("Enemy settings")]
        public EnemyMelee_Type meleeType;
        [SerializeField] private Transform shieldTransform;
        [SerializeField] private float dodgeCooldown;
        private float lastTimeDodge;
        
        [Header("Axe Throw ability ")] 
        public GameObject axePrefabs;
        public float axeFlySpeed;
        public float aimTimer;
        public float axeThrowCooldown;
        public Transform axeStartPoint;
        public float lastTimeAxeThrown;
        
        [FormerlySerializedAs("attackDataMelee")] [FormerlySerializedAs("attackData")] [Header("AttackData")] 
        public AttackData_EnemyMelee attackDataEnemyMelee;
        public List<AttackData_EnemyMelee> attackList;
        
        protected override void Awake()
        {
            base.Awake();
            idleState = new IdleState_Melee(this, stateMachine, Constant.AnimationParameter.IdleState);
            moveState = new MoveState_Melee(this, stateMachine, Constant.AnimationParameter.MoveState);
            recoveryState = new RecoveryState_Melee(this, stateMachine, Constant.AnimationParameter.RecoveryState);
            chaseState = new ChaseState_Melee(this, stateMachine, Constant.AnimationParameter.ChaseState);
            attackState = new AttackState_Melee(this, stateMachine, Constant.AnimationParameter.AttackState);
            deadState = new DeadState_Melee(this, stateMachine, Constant.AnimationParameter.IdleState);
            abilityState = new AbilityState_Melee(this, stateMachine, "Ability");

            enemyVisuals = GetComponent<Enemy_Visuals>();
        }
        protected override void Start()
        {
            base.Start();
            InitializeEnemy();
            stateMachine.Initialize(idleState);
            ResetCooldown();
            
            enemyVisuals.SetupLook();
            UpdateAttackData();
        }
        protected override void Update()
        {
            base.Update();
            stateMachine.currentState.OnExecute();
            
            if (ShouldEnterBattleMode())
                EnterBattleMode();
        }
        private void InitializeEnemy()
        {
            if (meleeType == EnemyMelee_Type.AxeThrow)
            {
                enemyVisuals.SetupWeaponType(Enemy_MeleeWeaponType.Throw);
            }
            
            if (meleeType == EnemyMelee_Type.Shield)
            {
                anim.SetFloat(Constant.AnimationParameter.ChaseIndex, 1);
                shieldTransform.gameObject.SetActive(true);
                enemyVisuals.SetupWeaponType(Enemy_MeleeWeaponType.OneHand);
            }
            
            if (meleeType == EnemyMelee_Type.Dodge)
            {
                enemyVisuals.SetupWeaponType(Enemy_MeleeWeaponType.Unarmed);
            }
        }

        public void UpdateAttackData()
        {
            Enemy_WeaponModel currentWeapon = enemyVisuals.currentWeaponModel.GetComponent<Enemy_WeaponModel>();

            if (currentWeapon.weaponData != null)
            {
                attackList = new List<AttackData_EnemyMelee>(currentWeapon.weaponData.attackData);
                turnSpeed = currentWeapon.weaponData.turnSpeed;
            }
        }
        public override void EnterBattleMode()
        {
            if (inBattleMode)
                return;

            base.EnterBattleMode();
            stateMachine.ChangeState(recoveryState);
        }
        public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.transform.position) < attackDataEnemyMelee.attackRange;
        public void EnableWeaponModel(bool active)
        {
            enemyVisuals.currentWeaponModel.gameObject.SetActive(active);
        }
        public override void GetHit()
        {
            base.GetHit();
            if(healthPoint <= 0)
                stateMachine.ChangeState(deadState);
        }
        private void ResetCooldown()
        {
            lastTimeDodge -= dodgeCooldown;
            lastTimeAxeThrown -= axeThrowCooldown;
        }

        public void ActivateDodge()
        {
            if(meleeType != EnemyMelee_Type.Dodge)
                return;
            if(stateMachine.currentState != chaseState)
                return;
            if (Vector3.Distance(transform.position, player.transform.position) < 2f)
                return;
            
            float dodgeAnimationDuration = GetAnimationClipDuration("Dodge roll");
            
            if (Time.time > dodgeCooldown + dodgeAnimationDuration + lastTimeDodge)
            {
                lastTimeDodge = Time.time;
                anim.SetTrigger(Constant.AnimationParameter.Dodge);
            }
        }
        private float GetAnimationClipDuration(string clipName)
        {
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

            foreach(AnimationClip clip in clips)
            {
                if (clip.name == clipName)
                    return clip.length;
            }

            Debug.Log(clipName + "animation not found!");
            return 0;
        }
        public override void AbilityTrigger()
        {
            base.AbilityTrigger();

            moveSpeed = moveSpeed * .6f;
            EnableWeaponModel(false);
        }
        public bool CanThrowAxe()
        {
            if (meleeType != EnemyMelee_Type.AxeThrow)
                return false;

            if (Time.time > lastTimeAxeThrown + axeThrowCooldown)
            {
                lastTimeAxeThrown = Time.time;
                return true;
            }

            return false;
        }
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDataEnemyMelee.attackRange);
            
        }
    }
}
