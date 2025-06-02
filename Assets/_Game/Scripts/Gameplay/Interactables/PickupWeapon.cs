using _Game.Scripts.Gameplay.MainCharacter;
using _Game.Scripts.Gameplay.Weapon;
using _Game.Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Interactables
{
    public class PickupWeapon : Interactable
    {
        [SerializeField] private BackupWeaponModel[] models;
        [SerializeField] private Weapon.Weapon weapon;
        [SerializeField] private Weapon_Data weaponData;

        private bool oldWeapon;

        private void Start()
        {
            if (oldWeapon == false)
                weapon = new Weapon.Weapon(weaponData);
            SetupGameObject();
        }
        [ContextMenu("Update models")]
        private void SetupGameObject()
        {
            gameObject.name = "Pickup_Weapon - " + weaponData.weaponType.ToString();
            SetupWeaponModel();
        }
        public void SetupPickupWeapon(Weapon.Weapon weapon, Transform transform)
        {
            oldWeapon = true;
            this.weapon = weapon;
            weaponData = weapon.WeaponData;
            this.transform.position = transform.position + new Vector3(0, .75f, 0);
        }
        private void SetupWeaponModel()
        {
            foreach (BackupWeaponModel model in models)
            {
                model.gameObject.SetActive(false);

                if (model.weaponType == weapon.weaponType)
                {
                    model.gameObject.SetActive(true);
                    UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());
                }
            }
        }
        public override void Interaction()
        {
            weaponController.PickUpWeapon(weapon);
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
}
