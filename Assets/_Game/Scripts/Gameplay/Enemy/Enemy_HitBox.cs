using _Game.Scripts.Gameplay.Health;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy
{
    public class Enemy_HitBox : HitBox
    {
        private Enemy enemy;

        protected override void Awake()
        {
            base.Awake();

            enemy = GetComponentInParent<Enemy>();
        }

        public override void TakeDamage(float damage)
        {
            int newDamage = Mathf.RoundToInt(damage * damageMultiplier);

            enemy.GetHit(newDamage);
        }
    }
}
