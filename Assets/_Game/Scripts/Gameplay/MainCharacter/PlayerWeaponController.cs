using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Gameplay.Weapon;
using _Game.Scripts.Systems;
using _Game.Scripts.Utilities;
using UnityEngine;


namespace _Game.Scripts.Gameplay.MainCharacter
{
    public class PlayerWeaponController : MonoBehaviour
    {
        private Player player;
        private const float REFERENCE_BULLET_SPEED = 20;
        //This is the default speed from whcih our mass formula is derived.
        
        [SerializeField] private Weapon.Weapon currentWeapon;
        private bool weaponReady;
        private bool isShooting;
        
        [Header("Bullet details")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private Transform weaponHolder;

        [Header("Inventory")] 
        [SerializeField] private List<Weapon.Weapon> weaponSlots;
        [SerializeField] private int maxSlots;
        
        private void Start()
        {
            player = GetComponent<Player>();
            AssignInputEvents();
            Invoke(nameof(EquipStartingWeapon),1f);
            currentWeapon.bulletsInMagazine = currentWeapon.totalReserveAmmo;
        }

        private void Update()
        {
            if(isShooting)
                Shoot();
        }

        private void EquipStartingWeapon()
        {
            EquipWeapon(0);
        }

        private void Shoot()
        {
            if (currentWeapon.CanShoot() == false || !weaponReady)
            {
                return;
            }
            
            
            if(currentWeapon.shootType == ShootType.Single)
                isShooting = false;
            if(currentWeapon.BurstActivated() == true)
                StartCoroutine(BurstFire());
            else
                FireSingleBullet();
            currentWeapon.ReduceBulletsInMagazine();
            player.weaponVisuals.PlayerFireAnimation();
        }

        private void FireSingleBullet()
        {
            GameObject newBullet = ObjectPool.Instance.GetBullet();
            newBullet.transform.position = GunPoint().position;
            newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);
            
            Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
            
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            bulletScript.BulletSetup(currentWeapon.gunDistance);
            
            Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());
            rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
            rbNewBullet.velocity = bulletDirection * bulletSpeed;
        }

        private IEnumerator BurstFire()
        {
            SetWeaponReady(false);
            for (int i = 0; i < currentWeapon.burstModePerShoot; i++)
            {
                FireSingleBullet();
                 yield return new WaitForSeconds(currentWeapon.burstFireDelay);
            }
            SetWeaponReady(true);
        }
        private void Reload()
        {
            SetWeaponReady(false);
            player.weaponVisuals.PlayReloadAnimation();
        }

        public Vector3 BulletDirection()
        {
            Transform aim = player.aim.Aim();

            Vector3 direction = (aim.position - GunPoint().position).normalized;

            if(player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
                direction.y = 0;
            
            return direction;
        }

        public Transform GunPoint() => player.weaponVisuals.CurrentWeaponModel().gunPoint;
        public Weapon.Weapon CurrentWeapon() => currentWeapon;
        
        public bool HasOnlyOneWeapon()=>weaponSlots.Count <= 1;

        public Weapon.Weapon WeaponInSlots(WeaponType weaponType)
        {
            foreach (var weapon in weaponSlots)
            {
                if(weapon.weaponType == weaponType)
                    return weapon;
            }
            return null;
        }
        #region SlotsManagement - Pickup/Equip/Drop/Ready/... Weapon
        private void EquipWeapon(int i)
        {
            if (i >= weaponSlots.Count)
                return;

            SetWeaponReady(false);

            currentWeapon = weaponSlots[i];
            player.weaponVisuals.PlayWeaponEquipAnimation();

            CameraManager.Instance.ChangeCameraDistance(currentWeapon.cameraDistance);
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

        public void SetWeaponReady(bool ready) => weaponReady = ready;
        public bool WeaponReady() => weaponReady;
        #endregion
        #region AssignInput

        private void AssignInputEvents()
        {
            PlayerControls controls = player.controls;
            controls.Character.Fire.performed += context => isShooting = true;
            controls.Character.Fire.canceled += context => isShooting = false;
            controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
            controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
            controls.Character.EquipSlot3.performed += context => EquipWeapon(2);
            controls.Character.EquipSlot4.performed += context => EquipWeapon(3);
            controls.Character.EquipSlot5.performed += context => EquipWeapon(4);
            controls.Character.Drop.performed += context => DropWeapon();
            controls.Character.ToogleWeaponMode.performed += context => 
            controls.Character.Reload.performed += context => currentWeapon.ToggleBurst();
            {
                if (currentWeapon.CanReload())
                {
                    if(currentWeapon.CanReload() && WeaponReady())
                        Reload();
                }
            };
        }
        #endregion
    }
}
