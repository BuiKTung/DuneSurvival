using _Game.Scripts.Gameplay.Health;
using UnityEngine;

namespace _Game.Scripts.Gameplay.MainCharacter
{
    public class PlayerHitBox : HitBox
    {
        private Player player;
        protected override void Awake()
        {
            base.Awake();

            player = GetComponentInParent<Player>();
        }

        public override void TakeDamage(float damage)
        {
            int newDamage = Mathf.RoundToInt(damage * damageMultiplier);

            player.health.ReduceHealth(newDamage);
        }
    }
}
