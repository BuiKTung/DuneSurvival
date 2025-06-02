using System;
using System.Collections.Generic;
using _Game.Scripts.Gameplay.Interactables;
using UnityEngine;

namespace _Game.Scripts.Gameplay.MainCharacter
{
    public class PlayerInteraction : MonoBehaviour
    {
        private List<Interactable> interactables = new List<Interactable>();
        public List<Interactable> GetInteractables() => interactables;
        
        private Interactable closestInteractable;

        private void Start()
        {
            Player player = GetComponent<Player>();
            player.controls.Character.Interact.performed += context => InteracWithClosest();
        }

        private void InteracWithClosest()
        {
            closestInteractable?.Interaction();
            interactables.Remove(closestInteractable);
            
            UpdateClosestInteractable();
        }

        public void UpdateClosestInteractable()
        {
            closestInteractable?.HighLightActive(false);
            closestInteractable = null;
            float closestDistance = float.MaxValue;
            
            foreach (var var in interactables)
            {
                float distance = Vector3.Distance(transform.position, var.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = var;
                }
            }
            
            closestInteractable?.HighLightActive(true);
        }
    }
}
