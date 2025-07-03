using _Game.Scripts.Gameplay.Health;
using _Game.Scripts.Utilities;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Weapon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private BoxCollider cd;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private TrailRenderer trailRenderer;
        
        [SerializeField] private GameObject bulletImpactFX;
        
        private Vector3 startPosition;
        private float flyDistance;
        private bool bulletDisabled;
        
        public float impactForce;
        private int bulletDamage;
        private void Update()
        {
            FadeTrailIfNeeded();
            DisableBulletIfNeeded();
            ReturnToPoolIfNeeded();
        }
        private void ReturnToPoolIfNeeded()
        {
            if(trailRenderer.time < 0)
            {
                ObjectPool.Instance.ReturnToPool(gameObject);
            }
        }

        private void DisableBulletIfNeeded()
        {
            if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
            {
                cd.enabled = false;
                meshRenderer.enabled = false;
                bulletDisabled = true;
            }
        }

        private void FadeTrailIfNeeded()
        {
            if (Vector3.Distance(startPosition, transform.position) > flyDistance)
            {
                trailRenderer.time -= 2*Time.deltaTime;
            }
        }

        public void BulletSetup(int bulletDamage, float flyDistance = 100, float impactForce = 100)
        {
            this.impactForce = impactForce;
            this.bulletDamage = bulletDamage;
            bulletDisabled = false;
            cd.enabled = true;
            meshRenderer.enabled = true;
            trailRenderer.time = .25f;
            startPosition = transform.position;
            this.flyDistance = flyDistance + .5f;
        }
        private void OnCollisionEnter(Collision collision)
        {
            CreateImpactFx();
            ObjectPool.Instance.ReturnToPool(gameObject);
            
            EnemyShield shield = CacheComponent<EnemyShield>.CacheGetComponent(collision.gameObject);
            
            if (shield != null )
            {
                shield.ReduceDurability();
                return;
            }
            
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            damagable?.TakeDamage(bulletDamage);
            ApplyBulletImpactToEnemy(collision);
        }
        private void ApplyBulletImpactToEnemy(Collision collision)
        {
            Enemy.Enemy enemy = CacheComponent<Enemy.Enemy>.CacheGetComponentInParent(collision.gameObject);
            if (enemy != null)
            {
                Vector3 force = rb.velocity.normalized * impactForce;
                Rigidbody hitRigidbody = collision.collider.attachedRigidbody;
                enemy.BulletImpact(force, collision.contacts[0].point, hitRigidbody);
            }
        }

        private void CreateImpactFx()
        {
                GameObject newImpactFx = ObjectPool.Instance.GetObject(bulletImpactFX,transform);
                ObjectPool.Instance.ReturnToPoolWaitASecond(newImpactFx,1f);
        }
    }
}
