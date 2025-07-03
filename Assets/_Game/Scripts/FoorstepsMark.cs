using UnityEngine;

namespace _Game.Scripts
{
    public class FoorstepsMark : MonoBehaviour
    {
        [Header("Trail Settings")]
        [SerializeField] private TrailRenderer leftFoot;
        [SerializeField] private TrailRenderer rightFoot;

        [Header("Ground Detection")]
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField, Range(0.01f, 0.3f)] private float checkRadius = 0.05f;
        [SerializeField, Range(-0.15f, 0.15f)] private float rayDistance = -0.05f;

        private void Update()
        {
            CheckFootstep(leftFoot);
            CheckFootstep(rightFoot);
        }

        private void CheckFootstep(TrailRenderer foot)
        {
            if (foot == null) return;

            Vector3 checkPos = foot.transform.position + Vector3.down * rayDistance;
            bool touchingGround = Physics.CheckSphere(checkPos, checkRadius, whatIsGround);
            foot.emitting = touchingGround;
        }

        private void OnDrawGizmos()
        {
            DrawFootGizmos(leftFoot?.transform);
            DrawFootGizmos(rightFoot?.transform);
        }

        private void DrawFootGizmos(Transform foot)
        {
            if (foot == null) return;
            Gizmos.color = Color.cyan;
            Vector3 checkPos = foot.position + Vector3.down * rayDistance;
            Gizmos.DrawWireSphere(checkPos, checkRadius);
        }
    }
}
