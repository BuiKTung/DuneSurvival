using UnityEngine;
using UnityEngine.AI;

namespace _Game.Scripts.Gameplay.Enemy.StateMachine
{
    public class EnemyState : IState
    {
        protected Enemy enemyBase;
        protected EnemyStateMachine stateMachine;

        private string animBoolName;
        
        protected float stateTimer;

        protected bool triggerCalled;

        protected EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
        {
            this.enemyBase = enemyBase;
            this.stateMachine = stateMachine;
            this.animBoolName = animBoolName;
        }

        public virtual void OnEnter()
        {
            enemyBase.anim.SetBool(animBoolName, true);
            triggerCalled = false;
        }
        public virtual void OnExecute()
        {
            stateTimer -= Time.deltaTime;
        }
        public virtual void Exit()
        {
            enemyBase.anim.SetBool(animBoolName, false);
        }
        public void AnimationTrigger() => triggerCalled = true;
        public virtual void AbilityTrigger() 
        {
        }
        protected Vector3 GetNextPathPoint()
        {
            NavMeshAgent agent = enemyBase.agent;
            if (!agent.CalculatePath(agent.destination, enemyBase.cachedPath) || enemyBase.cachedPath.corners.Length < 2)
            {
                return agent.destination;
            }

            for (int i = 0; i < enemyBase.cachedPath.corners.Length - 1; i++)
            {
                if (Vector3.Distance(agent.transform.position, enemyBase.cachedPath.corners[i]) < 1f)
                {
                    return enemyBase.cachedPath.corners[i + 1];
                }
            }

            return agent.destination;
        }
    }
}
