using UnityEngine;

namespace _Game.Scripts.Gameplay.Health
{
    public class Ragdoll : MonoBehaviour
    {
        [SerializeField] private Transform ragdollParent;
        
        [SerializeField] private Collider[] ragdollColliders;
        [SerializeField] private Rigidbody[] ragdollRigidbodies;

        private void Awake()
        {
            ragdollColliders = GetComponentsInChildren<Collider>();
            ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            
            RagdollActive(false);
        }

        public void RagdollActive(bool active)
        {
            foreach (var VARIABLE in ragdollRigidbodies)
            {
                VARIABLE.isKinematic = !active;
            }
        }

        public void ColliderActive(bool active)
        {
            foreach (var VARIABLE in ragdollColliders)
            {
                VARIABLE.enabled = active;
            }
        }
        
    }
}
