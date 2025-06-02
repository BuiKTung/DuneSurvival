using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Gameplay.Weapon
{
   [System.Serializable]
   public class Weapon
   {
      public WeaponType weaponType;
      public Weapon_Data WeaponData { get; private set; }
      #region Regular mode variables
      public ShootType shootType;
      public int bulletsPerShot { get; private set; }
      private float defaultFireRate;
      public float fireRate = 1; // bullets per second
      private float lastShootTime;
      #endregion
      #region Burst mode  variables
      private bool burstAvalible;
      public bool burstActive;

      private int burstBulletsPerShot;
      private float burstFireRate;
      public float burstFireDelay { get; private set; }
      #endregion

      [Header("Magazine details")]
       public int bulletsInMagazine;
       public int magazineCapacity;
       public int totalReserveAmmo;

       #region Weapon generic info variables
       
       public float reloadSpeed { get; private set; } // how fast charcater reloads weapon    
       public float equipmentSpeed { get; private set; } // how fast character equips weapon
       public float gunDistance { get; private set; }
       public float cameraDistance {get; private set;} 
       #endregion
       #region Weapon spread variables
       [Header("Spread ")] 
       private float baseSpread = 1;
       private float maximumSpread = 3;
       private float currentSpread = 2;

       private float spreadIncreaseRate = .15f;

       private float lastSpreadUpdateTime;
       private float spreadCooldown = 1;

       #endregion
       public Weapon(Weapon_Data weaponData)
       {

           bulletsInMagazine = weaponData.bulletsInMagazine;
           magazineCapacity = weaponData.magazineCapacity;
           totalReserveAmmo = weaponData.totalReserveAmmo;

           weaponType = weaponData.weaponType;
           shootType = weaponData.shootType;
           burstAvalible = weaponData.burstAvalible;
           burstActive = weaponData.burstActive;
           burstBulletsPerShot = weaponData.burstBulletsPerShot;
           burstFireRate = weaponData.burstFireRate;
           burstFireDelay = weaponData.burstFireDelay;
           if (weaponType == WeaponType.Shotgun)
           {
              bulletsPerShot = burstBulletsPerShot;
              fireRate = burstFireRate;
              burstFireDelay = 0;
           }
           else
           {
              bulletsPerShot = weaponData.bulletsPerShot;
              fireRate = weaponData.fireRate;
           }

           baseSpread = weaponData.baseSpread;
           maximumSpread = weaponData.maxSpread;
           spreadIncreaseRate = weaponData.spreadIncreaseRate;


           reloadSpeed = weaponData.reloadSpeed;
           equipmentSpeed = weaponData.equipmentSpeed;
           gunDistance = weaponData.gunDistance;
           cameraDistance = weaponData.cameraDistance;

           defaultFireRate = fireRate;
           this.WeaponData = weaponData;
       }
      public void ReduceBulletsInMagazine()
      {
         if (weaponType == WeaponType.Shotgun)
         {
            bulletsInMagazine--;
            return;
         }
         bulletsInMagazine -= bulletsPerShot;
      }
      public bool CanShoot()
      {
         if (HaveEnoughBullets() && ReadyToFire())
         {
            return true;
         }
         return false;
      }
      private bool ReadyToFire()
      {
         if (Time.time > lastShootTime + 1f / fireRate)
         {
            lastShootTime = Time.time; 
            return true;
         }
         return false;
      }

      #region Burst Methods

      public bool BurstActivated()
      {
         if (weaponType == WeaponType.Shotgun)
         {
            return true;
         }
         return burstActive;
      }

      public void ToggleBurst()
      {
         if(burstActive == false)
            return;
         burstActive = !burstActive;
         if (burstActive)
         {
            bulletsPerShot = burstBulletsPerShot;
            fireRate = burstFireRate;
         }
         else
         {
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
         }
      }

      #endregion
      
      #region Spread Methods
      public Vector3 ApplySpread(Vector3 originalDirection)
      {
         UpdateSpread();
         float randomizedValue = Random.Range(-currentSpread, currentSpread);
         
         Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);
         
         return spreadRotation * originalDirection;
      }

      private void UpdateSpread()
      {
         if (Time.time - lastSpreadUpdateTime > spreadCooldown)
         {
            currentSpread = baseSpread;
         }
         else
         {
            IncreaseSpread();
         }
         lastSpreadUpdateTime = Time.time;
      }
      public void IncreaseSpread()
      {
         currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
      }
      #endregion

      #region Reload Methods
      public bool CanReload()
      {
         if(bulletsInMagazine == magazineCapacity)
            return false;
         if (totalReserveAmmo > 0)
         {
            return true;
         }
         return false;
      }

      public void ReloadBullets()
      {
         totalReserveAmmo += bulletsInMagazine;
         int bulletsToReload = magazineCapacity;
         if(bulletsToReload > totalReserveAmmo)
            bulletsToReload = totalReserveAmmo;
         bulletsInMagazine = bulletsToReload;
         totalReserveAmmo -= bulletsToReload;
         if(totalReserveAmmo < 0)
            totalReserveAmmo = 0;
      }

      private bool HaveEnoughBullets() => bulletsInMagazine > 0;
      #endregion
   }
   //Enum class
   public enum WeaponType
   {
      Piston,
      Revolver,
      AutoRifle,
      Shotgun,
      Rifle
   }

   public enum ShootType
   {
      Single,
      Auto,
   }

}
