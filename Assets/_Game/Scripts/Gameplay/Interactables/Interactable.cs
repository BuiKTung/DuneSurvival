using _Game.Scripts.Gameplay.MainCharacter;
using _Game.Scripts.Utilities;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Interactables
{
    public class Interactable : MonoBehaviour
    {
        /// <summary>
        /// We need cache Component for this situation
        /// </summary>
        
        [SerializeField] protected MeshRenderer meshRenderer;
        [SerializeField] private Material highlightMaterial;
        private Material defaultMaterial;
        protected PlayerWeaponController weaponController;

        private void Start()
        {
            if(meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();
            defaultMaterial = meshRenderer.sharedMaterial;
        }

        public void HighLightActive(bool active)
        {
            if(active)
                meshRenderer.material = highlightMaterial;
            else
                meshRenderer.material = defaultMaterial;
        }

        protected void UpdateMeshAndMaterial(MeshRenderer newMeshRenderer)
        {
            meshRenderer = newMeshRenderer;
            defaultMaterial = newMeshRenderer.sharedMaterial;
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (weaponController == null)
                weaponController = CacheComponent<PlayerWeaponController>.CacheGetComponent(other.gameObject);

            PlayerInteraction playerInteraction = CacheComponent<PlayerInteraction>.CacheGetComponent(other.gameObject);
        
            if(playerInteraction == null )
                return;
        
            HighLightActive(true);
            playerInteraction.GetInteractables().Add(this);
            playerInteraction.UpdateClosestInteractable();
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            PlayerInteraction playerInteraction = CacheComponent<PlayerInteraction>.CacheGetComponent(other.gameObject);
        
            if(playerInteraction == null)
                return;
        
            HighLightActive(false);
            playerInteraction.GetInteractables().Remove(this);
            playerInteraction.UpdateClosestInteractable();
        }

        public virtual void Interaction()
        {
        }
    }
}
