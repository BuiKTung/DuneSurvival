using _Game.Scripts.Gameplay.Health;
using _Game.Scripts.Systems;
using UnityEngine;

namespace _Game.Scripts.Gameplay.MainCharacter
{
    public class PlayerHealth : HealthController
    {
        private Player player;

        public bool isDead { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<Player>();
        }

        public override void ReduceHealth(int damage)
        {
            base.ReduceHealth(damage);

            if (ShouldDie())
                Die();
            
            UI.UI.Instance.inGameUI.UpdateHealthUI(currentHealth,maxHealth);
            Debug.Log("");
        }

        [ContextMenu("Take Damage")]
        public void TakeDamage()
        {
            ReduceHealth(1);
        }
        private void Die()
        {
            if (isDead)
                return;
            
            Debug.Log("Player was killed at " + Time.time);
            isDead = true;
            player.aim.enabled = false;
            player.ragdoll.RagdollActive(true);

            GameManager.Instance.GameOver();
        }    
    }
}
