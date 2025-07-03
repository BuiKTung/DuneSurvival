using System;
using System.Collections;
using _Game.Scripts.Gameplay.Enemy.StateMachine;
using _Game.Scripts.Gameplay.Health;
using _Game.Scripts.Gameplay.MainCharacter;
using _Game.Scripts.Gameplay.Mission;
using _Game.Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace _Game.Scripts.Gameplay.Enemy
{
    public enum EnemyType
    {
        Melee, 
        Ranged,
        Boss,
        Random
    }
    public class Enemy : MonoBehaviour
    {
        public EnemyType enemyType;
        public LayerMask whatIsAlly;
        public LayerMask whatIsPlayer;
        
        [Header("Idle data")] 
        public float idleTime;
        public float aggresionRange;

        [Header("Move data")] 
        public float moveSpeed;
        public float turnSpeed;
        public float chaseSpeed;
        private bool manualMovement;
        private bool manualRotation;
        
        public Transform[] patrolPoints;
        private Vector3[] patrolPositions;
        private int currentPatrolIndex;
        protected bool inBattleMode { get; private set; }
        protected bool isMeleeAttackReady;

        public Player player { get; private set; }
        public NavMeshAgent agent { get; private set; }
        public EnemyStateMachine stateMachine { get; private set; }
        public Enemy_Health health { get; private set; }
        public Enemy_DropController dropController { get; private set; }
        public Animator anim { get; private set; }
        
        public NavMeshPath cachedPath;
        
        protected virtual void Awake()
        {
            stateMachine = new EnemyStateMachine();
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Enemy_Health>();
            anim = GetComponentInChildren<Animator>();
            dropController = GetComponent<Enemy_DropController>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        protected virtual void Start()
        {
            InitalizePatrolPoints();
            cachedPath = new NavMeshPath();
        }
        protected virtual void Update()
        {
        }
        public virtual void MeleeAttackCheck(Transform[] damagePoints, float attackCheckRadius,GameObject fx,int damage)
        {
            if (isMeleeAttackReady == false)
                return;

            foreach (Transform attackPoint in damagePoints)
            {
                Collider[] detectedHits =
                    Physics.OverlapSphere(attackPoint.position, attackCheckRadius, whatIsPlayer);


                for (int i = 0; i < detectedHits.Length; i++)
                {
                    IDamagable damagable = detectedHits[i].GetComponent<IDamagable>();

                    if (damagable != null)
                    {

                        damagable.TakeDamage(damage);
                        isMeleeAttackReady = false;
                        GameObject newAttackFx = ObjectPool.Instance.GetObject(fx, attackPoint);

                        ObjectPool.Instance.ReturnToPoolWaitASecond(newAttackFx, 1);
                        return;
                    }
                }

            }
        }
        public void EnableMeleeAttackCheck(bool enable) => isMeleeAttackReady = enable;
        public bool PlayerInAggresionRange() => Vector3.Distance(transform.position, player.transform.position) <= aggresionRange;
        protected bool ShouldEnterBattleMode()
        {
            bool inAggresionRange = Vector3.Distance(transform.position, player.transform.position) < aggresionRange;

            if (inAggresionRange && !inBattleMode)
            {
                EnterBattleMode();
                return true;
            }

            return false;
        }

        protected virtual void EnterBattleMode()
        {
            inBattleMode = true;
        }
        public virtual void GetHit(int damage)
        {
            health.ReduceHealth(damage);
            if (health.ShouldDie())
                Die();
            EnterBattleMode();
        }

        protected virtual void Die()
        {
            dropController.DropItems();

            MissionObject_HuntTarget huntTarget = GetComponent<MissionObject_HuntTarget>();
            huntTarget?.InvokeOnTargetKilled();
        }
        public virtual void BulletImpact(Vector3 force, Vector3 hitpoint, Rigidbody rb)
        {
            if(health.ShouldDie())
                StartCoroutine(DeathImpactCoroutine(force,hitpoint,rb));
            //StartCoroutine(DeathImpactCoroutine(force, hitpoint, rb));
        }

        private IEnumerator DeathImpactCoroutine(Vector3 force, Vector3 hitpoint, Rigidbody rb)
        {
            yield return new WaitForSeconds(0.1f);
            rb.AddForceAtPosition(force, hitpoint, ForceMode.Impulse);
        }
        public void FaceTarget(Vector3 target)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            Vector3 currentEulerAngels = transform.rotation.eulerAngles;
            float yRotation = Mathf.LerpAngle(currentEulerAngels.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(currentEulerAngels.x, yRotation, currentEulerAngels.z);
        }
        #region Animation events
        public void ActivateManualMovement(bool value) => this.manualMovement = value;
        public bool ManualMovement() => manualMovement;
        public void ActivateManualRotation(bool value) => this.manualRotation = value;
        public bool ManualRotation() => manualRotation;
        public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
        public virtual void AbilityTrigger()
        {
            stateMachine.currentState.AbilityTrigger();
        }
        #endregion
        #region Patrol logic
        public Vector3 GetPatrolDestination()
        {
            Vector3 destination = patrolPositions[currentPatrolIndex];

            currentPatrolIndex++;

            if (currentPatrolIndex >= patrolPoints.Length)
                currentPatrolIndex = 0;

            return destination;
        }
        private void InitalizePatrolPoints()
        {
            patrolPositions = new Vector3[patrolPoints.Length];

            for (int i = 0; i < patrolPoints.Length; i++)
            {
                patrolPositions[i] = patrolPoints[i].position;
                patrolPoints[i].gameObject.SetActive(false);
            }
        }
        #endregion
        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, aggresionRange);
        }

        public void MakeEnemyVip()
        {
            int additionalHealth = Mathf.RoundToInt(health.currentHealth * 1.5f);
            health.currentHealth += additionalHealth;
            
            transform.localScale = transform.localScale * 1.5f;
        }

        public void EnemyInLastDefendMisson(Vector3 defencePoint)
        {
            agent.SetDestination(defencePoint);
            aggresionRange = 100;
        }
    }
}