using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Gameplay.Weapon
{
   [System.Serializable]
   public class Weapon 
   {
      public WeaponType weaponType;
      [Range(2, 12)] public float gunDistance = 4;
      [Range(3f, 8f)] public float cameraDistance = 6f;

      [Header("Shoot details")] 
      public ShootType shootType;
      public int bulletsPerShot;
      public int defaultFireRate;
      public float fireRate = 1; //bullet per second 
      private float lastShootTime = Mathf.NegativeInfinity;

      [Header("BurstShoot details")] 
      public bool burstAvalible;
      public bool burstActive;
      
      public int burstModePerShoot;
      public float burstModeFireRate;
      public float burstFireDelay = .1f;
      
      [Header("Magazine details")]
      public int bulletsInMagazine;
      public int magazineCapacity;
      public int totalReserveAmmo;
      
      [Range(1,3)]
      public float reloadSpeed = 1;
      [Range(1,3)]
      public float equipSpeed = 1;

      
      [Header("Spread details")] 
      private float currentSpread = 2f;
      public float baseSpread = 0.5f;
      public float maxSpread = 3f;
      
      public float  spreadIncreaseRate = .15f;
      private float lastSpreadUpdateTime;
      private float spreadCooldown = 1;
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
            burstFireDelay = 0;
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
            bulletsPerShot = burstModePerShoot;
            fireRate = burstModeFireRate;
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
         currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maxSpread);
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
