using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Gameplay.Interactables;
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
        [SerializeField] private Weapon_Data defaultWeaponData;
        [SerializeField] private Weapon.Weapon currentWeapon;
        [SerializeField] private bool weaponReady;
        [SerializeField] private bool isShooting;
        
        [Header("Bullet details")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private Transform weaponHolder;

        [Header("Inventory")] 
        [SerializeField] private List<Weapon.Weapon> weaponSlots;
        [SerializeField] private int maxSlots;
        
        [SerializeField] private GameObject weaponPickupPrefab;
        
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
            GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab);
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
            for (int i = 0; i < currentWeapon.bulletsPerShot; i++)
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
        
        private void EquipStartingWeapon()
        {
            weaponSlots[0] = new Weapon.Weapon(defaultWeaponData);
            EquipWeapon(0);
        }

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
            if (WeaponInSlots(newWeapon.weaponType) != null)
            {
                WeaponInSlots(newWeapon.weaponType).totalReserveAmmo += newWeapon.bulletsInMagazine;
                return;
            }
            
            if (weaponSlots.Count >= maxSlots && newWeapon.weaponType != currentWeapon.weaponType)
            {
                int weaponIndex = weaponSlots.IndexOf(currentWeapon);
                weaponSlots[weaponIndex] = newWeapon;
                
                CreateWeaponOnTheGround();
                EquipWeapon(weaponIndex);
                return;
            }

            
            weaponSlots.Add(newWeapon);
            player.weaponVisuals.SwitchOnBackupWeaponModel();
        }
        private void DropWeapon()
        {
            if(weaponSlots.Count <= 1){return;}

            CreateWeaponOnTheGround();

            weaponSlots.Remove(currentWeapon);
            EquipWeapon(0);
        }

        private void CreateWeaponOnTheGround()
        {
            GameObject droppedWeapon = ObjectPool.Instance.GetObject(weaponPickupPrefab);
            droppedWeapon.GetComponent<PickupWeapon>()?.SetupPickupWeapon(currentWeapon, transform);
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
            controls.Character.ToogleWeaponMode.performed += context => currentWeapon.ToggleBurst();
            controls.Character.Reload.performed += context => 
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
