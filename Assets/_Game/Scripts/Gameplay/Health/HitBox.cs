using UnityEngine;

namespace _Game.Scripts.Gameplay.Health
{
    public class HitBox : MonoBehaviour,IDamagable
    {
        [SerializeField] protected float damageMultiplier = 1f;

        protected virtual void Awake()
        {

        }
        public virtual void TakeDamage(float damage)
        {
            
        }
    }
}
