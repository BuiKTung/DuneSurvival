using UnityEngine;

namespace _Game.Scripts.Gameplay.Mission
{
    public class MissionObject_EnemyRespawnPoint : MonoBehaviour
    {
        [SerializeField] MeshRenderer meshRenderer;
        void Start()
        {
            meshRenderer.enabled = false;
        }

   
    }
}
