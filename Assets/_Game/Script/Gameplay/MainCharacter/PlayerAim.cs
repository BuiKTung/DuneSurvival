
using UnityEngine;

namespace _Game.Script.Gameplay.MainCharacter
{
    public class PlayerAim : MonoBehaviour
    {
        private Player player;
        private PlayerControls controls;

        [Header("Aim Visual - Laser")] 
        [SerializeField] private LineRenderer aimLaser;
        
        [Header("Aim Information")]
        [SerializeField] private Transform aim;

        public Transform Aim() => aim;
        [SerializeField] private bool isAimingPrecisly;
        public bool CanAimPrecisly() => isAimingPrecisly;
        [SerializeField] private bool isLockingToTarget;
        [SerializeField] private LayerMask aimLayerMask;
        private Vector2 mouseInput;
        private RaycastHit lastKnownMouseHit;

        [Header("Camera Information")]
        [SerializeField] private Transform cameraTarget;
        [Range(0.7f, 1f)]
        [SerializeField] private float minCameraDistance;
        [Range(1.2f, 3f)]
        [SerializeField] private float maxCameraDistance;
        [Range(3f, 5f)]
        [SerializeField] private float cameraSensetivity = 5f;
        private void Start()
        {
            player = GetComponent<Player>();
            AssignInputEvents();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                isAimingPrecisly = !isAimingPrecisly;
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                isLockingToTarget = !isLockingToTarget;
            }

            UpdateAimVisual();
            UpdateAimPosition();
            UpdateCameraPosition();

        }

        private void UpdateAimVisual()
        {
            Transform gunPoint = player.weaponController.GunPoint();
            Vector3 laserDirection = player.weaponController.BulletDirection();
            float gunDistance = 4f;
            float laserTipLenght = .5f;
            
            Vector3 endPoint = gunPoint.position +laserDirection * gunDistance;

            if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunDistance))
            {
                endPoint = hit.point;
                laserTipLenght = 0;
            }
            aimLaser.SetPosition(0, gunPoint.position);
            aimLaser.SetPosition(1, endPoint);
            aimLaser.SetPosition(2, endPoint + laserTipLenght * laserDirection);
        }

        private void UpdateAimPosition()
        {
            Transform target = SetTarget();
            if (target != null && isLockingToTarget)
            {
                aim.position = target.position;
                return;
            }
            aim.position = GetMouseHitInfor().point;
            if (!isAimingPrecisly)
                aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
        }

        public RaycastHit GetMouseHitInfor()
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseInput);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
            {
                lastKnownMouseHit = hitInfo;
                return hitInfo;
            }

            return lastKnownMouseHit;
        }
        public Transform SetTarget()
        {
            Transform target = null;
            if (GetMouseHitInfor().transform.GetComponent<Target>() != null)
            {
                target = GetMouseHitInfor().transform;
            }
            return target;
        }

        #region Camera Region
        private Vector3 DesiredCameraPosition()
        {
            float actualMaxCameraDistance = player.movement.moveInput.y < -0.5f ? minCameraDistance : maxCameraDistance;

            Vector3 desiredCameraPosition = GetMouseHitInfor().point;
            Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

            float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
            float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);

            desiredCameraPosition = transform.position + aimDirection * clampedDistance;
            //desiredCameraPosition.y = transform.position.y + 1;
            return desiredCameraPosition;

        }
        private void UpdateCameraPosition()
        {
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), cameraSensetivity * Time.deltaTime);
        }
        #endregion
        /// <summary>
        /// Assign all input event for PlayerAim
        /// </summary>
        private void AssignInputEvents()
        {
            controls = player.controls;
            controls.Character.Aim.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
            controls.Character.Aim.canceled += _ => mouseInput = Vector2.zero;
        }
    }
}

