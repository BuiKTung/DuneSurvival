using System;
using System.Collections.Generic;
using _Game.Scripts.Utilities;
using UnityEngine;

namespace _Game.Scripts.Gameplay.MainCharacter
{
    public class PlayerWeaponController : MonoBehaviour
    {
        private const float REFERENCE_BULLET_SPEED = 20;
        //This is the default speed from whcih our mass formula is derived.
        private Player player;
        [SerializeField] private Weapon.Weapon currentWeapon;
        [Header("Bullet details")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private Transform gunPoint;
        [SerializeField] private Transform weaponHolder;

        [Header("Inventory")] 
        [SerializeField] private List<Weapon.Weapon> weaponSlots;
        [SerializeField] private int maxSlots;
        
        private void Start()
        {
            player = GetComponent<Player>();
            AssignInputEvents();
            currentWeapon = weaponSlots[0];
            currentWeapon.bulletsInMagazine = currentWeapon.totalReserveAmmo;
            
        }
        private void Shoot()
        {
            if (currentWeapon.CanShoot()== false)
            {
                return;
            }
            
            GameObject newBullet =
                Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

            Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

            rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
            rbNewBullet.velocity = BulletDirection() * bulletSpeed;

            Destroy(newBullet, 10);
            GetComponentInChildren<Animator>().SetTrigger(Constant.Fire);
        }

        public Vector3 BulletDirection()
        {
            Transform aim = player.aim.Aim();

            Vector3 direction = (aim.position - gunPoint.position).normalized;

            if(player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
                direction.y = 0;

            //weaponHolder.LookAt(aim);
            //gunPoint.LookAt(aim); TODO: find a better place for it. 

            return direction;
        }

        public Transform GunPoint() => gunPoint;
        public Weapon.Weapon CurrentWeapon() => currentWeapon;

        public Weapon.Weapon BackupWeapon()
        {
            foreach (Weapon.Weapon weapon in weaponSlots)
            {
                if (weapon != currentWeapon)
                    return weapon;
            }
            return null;
        }
        public bool HasOnlyOneWeapon()=>weaponSlots.Count <= 1;
        #region SlotsManagement - Pickup/Equip/Drop
        private void EquipWeapon(int i)
        {
            currentWeapon = weaponSlots[i];
            player.weaponVisuals.PlayWeaponEquipAnimation();
        }

        public void PickUpWeapon(Weapon.Weapon newWeapon)
        {
            if (weaponSlots.Count >= maxSlots)
            {
                return;
            }
            weaponSlots.Add(newWeapon);
            player.weaponVisuals.SwitchOnBackupWeaponModel();
        }
        /// <summary>
        /// !!!bug!!!: not show weapon 
        /// </summary>
        private void DropWeapon()
        {
            if(weaponSlots.Count <= 1){return;}

            weaponSlots.Remove(currentWeapon);
            EquipWeapon(0);
        }
        #endregion
        #region AssignInput

        private void AssignInputEvents()
        {
            PlayerControls controls = player.controls;
            controls.Character.Fire.performed += context => Shoot();
            controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
            controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
            controls.Character.Drop.performed += context => DropWeapon();
            controls.Character.Reload.performed += context =>
            {
                if (currentWeapon.CanReload())
                {
                    player.weaponVisuals.PlayReloadAnimation();
                }
            };
        }

        #endregion
    }
}
