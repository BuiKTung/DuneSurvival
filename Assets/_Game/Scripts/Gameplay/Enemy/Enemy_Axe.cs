using _Game.Scripts.Gameplay.MainCharacter;
using _Game.Scripts.Gameplay.Weapon;
using _Game.Scripts.Utilities;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy
{
    public class EnemyAxe : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform axeVisual;
        [SerializeField] private GameObject impactFx;
        
        [SerializeField] private float flySpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float timer;
        
        [SerializeField] private Vector3 direction;
        
        public void AxeSetup(float flySpeed, Transform player, float timer)
        {
            rotationSpeed = 1600;

            this.flySpeed = flySpeed;
            this.player = player;
            this.timer = timer;
        }
        private void Update()
        {
            axeVisual.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            timer -= Time.deltaTime;

            if (timer > 0)
                direction = player.position + Vector3.up - transform.position;


            rb.velocity = direction.normalized * flySpeed;
            transform.forward = rb.velocity;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            Player player = other.GetComponent<Player>();

            if (bullet != null || player != null)
            {
                GameObject newFx = ObjectPool.Instance.GetObject(impactFx);
                newFx.transform.position = transform.position;

                ObjectPool.Instance.ReturnToPool(gameObject);
                ObjectPool.Instance.ReturnToPoolWaitASecond(newFx, 1f);
            }
        }
    }
}
