using _Game.Scripts.Gameplay.Health;
using _Game.Scripts.Gameplay.MainCharacter;
using _Game.Scripts.Gameplay.Weapon;
using _Game.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Gameplay.Enemy
{
    public class Enemy_Axe : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform axeVisual;
        [SerializeField] private GameObject impactFx;
        
        [SerializeField] private float flySpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float aimTimer;
        [SerializeField] private float lifeTimer;
        [SerializeField] private float damage;
        
        [SerializeField] private Vector3 direction;
        
        public void AxeSetup(float flySpeed, Transform player, float aimTimer, float lifeTimer, int damage)
        {
            rotationSpeed = 1600;

            this.damage = damage;
            this.flySpeed = flySpeed;
            this.player = player;
            this.aimTimer = aimTimer;
            this.lifeTimer = lifeTimer;
        }
        private void Update()
        {
            axeVisual.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            
            lifeTimer -= Time.deltaTime;
            aimTimer -= Time.deltaTime;
            if (aimTimer > 0)
                direction = player.position + Vector3.up - transform.position;
            if (lifeTimer <= 0)
                DisableAxeThrown();
            
            transform.forward = rb.velocity;
        }
        private void FixedUpdate()
        {
            rb.velocity = direction.normalized * flySpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            damagable?.TakeDamage(damage);
            
            DisableAxeThrown();
        }

        private void DisableAxeThrown()
        {
            GameObject newFx = ObjectPool.Instance.GetObject(impactFx,transform);
            ObjectPool.Instance.ReturnToPool(gameObject);
            ObjectPool.Instance.ReturnToPoolWaitASecond(newFx, 1f);
        }
    }
}
