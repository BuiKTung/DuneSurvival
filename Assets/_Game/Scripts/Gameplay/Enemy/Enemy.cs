using System;
using System.Collections;
using _Game.Scripts.Gameplay.MainCharacter;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace _Game.Scripts.Gameplay.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [Header("Health data")] 
        [SerializeField]protected int healthPoint;
        
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
        public bool inBattleMode { get; private set; }
        private int currentPatrolIndex;
        public Player player { get; private set; }
        public NavMeshAgent agent { get; private set; }
        public EnemyStateMachine stateMachine { get; private set; }
        public Animator anim { get; private set; }
        public NavMeshPath cachedPath;
        protected virtual void Awake()
        {
            stateMachine = new EnemyStateMachine();
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponentInChildren<Animator>();
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
        public virtual void EnterBattleMode()
        {
            inBattleMode = true;
        }
        public virtual void GetHit()
        {
            EnterBattleMode();
            healthPoint--;
        }
        public virtual void DeathImpact(Vector3 force, Vector3 hitpoint, Rigidbody rb)
        {
            StartCoroutine(DeathImpactCoroutine(force, hitpoint, rb));
        }

        private IEnumerator DeathImpactCoroutine(Vector3 force, Vector3 hitpoint, Rigidbody rb)
        {
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(force, ForceMode.Impulse);
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
    }
}