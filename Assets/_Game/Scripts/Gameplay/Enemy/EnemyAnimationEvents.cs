using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy
{
    public class EnemyAnimationEvents : MonoBehaviour
    {
        private Enemy enemy;

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
    }
}
