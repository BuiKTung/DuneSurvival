using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy
{
    public class Enemy_AnimationEvents : MonoBehaviour
    {
        private Enemy enemy;
        public event Action OnAttackTriggered;
        public event Action OnRoarTriggered;

        private void Start()
        {
            enemy = GetComponentInParent<Enemy>();
        }

        public void AnimationTrigger() => enemy.AnimationTrigger();
        public void StartManualMovement() => enemy.ActivateManualMovement(true);
        public void StopManualMovement() => enemy.ActivateManualMovement(false);
        public void StartManualRotation() => enemy.ActivateManualRotation(true);
        public void StopManualRotation() => enemy.ActivateManualRotation(false);
        public void AbilityEvent() => enemy.GetComponent<Enemy_Melee>().AbilityTrigger();

        public void BeginMeleeRoar()
        {
            OnRoarTriggered?.Invoke();
        }
        public void BeginMeleeAttackCheck()
        {
            enemy?.EnableMeleeAttackCheck(true);
            OnAttackTriggered?.Invoke();
        }
        public void FinishMeleeAttackCheck()
        {
            enemy?.EnableMeleeAttackCheck(false);
        }
    }
}
