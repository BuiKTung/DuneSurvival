using _Game.Scripts.Core;
using Cinemachine;
using UnityEngine;

namespace _Game.Scripts.Systems
{
    public class CameraManager : Singleton<CameraManager>
    {
        private CinemachineVirtualCamera virtualCamera;
        private CinemachineFramingTransposer transposer;
        
        [Header("Camera Distance")]
        [SerializeField] private float targetCameraDistance;
        [SerializeField] private float distanceChangeRate;
        [SerializeField] private bool canChangeCameraDistance;
        
        protected override void Awake()
        {
            base.Awake();
            virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void Update()
        {
            UpdateCameraDistance();
        }

        private void UpdateCameraDistance()
        {
            if (canChangeCameraDistance == false)
                return;

            float currentDistnace = transposer.m_CameraDistance;

            if (Mathf.Abs(targetCameraDistance - currentDistnace) < .01f)
                return;
        
            transposer.m_CameraDistance =
                Mathf.Lerp(currentDistnace, targetCameraDistance, distanceChangeRate * Time.deltaTime);
        }

        public void ChangeCameraDistance(float distance)
        {
            transposer.m_CameraDistance = distance;
        }
    }
}
