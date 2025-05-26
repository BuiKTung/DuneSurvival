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

        public void BulletSetup(float flyDistance)
        {
            bulletDisabled = false;
            cd.enabled = true;
            meshRenderer.enabled = true;
            trailRenderer.time = .25f;
            startPosition = transform.position;
            this.flyDistance = flyDistance;
        }
        private void OnCollisionEnter(Collision collision)
        {
            CreateImpactFx(collision);
            ObjectPool.Instance.ReturnToPool(gameObject);
        }

        private void CreateImpactFx(Collision collision)
        {
            if (collision.contacts.Length > 0)
            {
                ContactPoint contact = collision.contacts[0];

                GameObject newImpactFx = ObjectPool.Instance.GetObject(bulletImpactFX);
                newImpactFx.transform.position = contact.point;
                
                ObjectPool.Instance.ReturnToPoolWaitASecond(newImpactFx,1f);
            }
        }
    }
}
