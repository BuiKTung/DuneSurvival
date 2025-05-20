using _Game.Scripts.Gameplay.MainCharacter;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Weapon
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<PlayerWeaponController>()?.PickUpWeapon(weapon);
        }
    }
}
