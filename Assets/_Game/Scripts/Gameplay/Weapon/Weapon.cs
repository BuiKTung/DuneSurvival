using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Gameplay.Weapon
{
   [System.Serializable]
   public class Weapon 
   {
      public WeaponType weaponType;
      public int bulletsInMagazine;
      public int magazineCapacity;
      public int totalReserveAmmo;
      
      [Range(1,3)]
      public float reloadSpeed = 1;
      [Range(1,3)]
      public float equipSpeed = 1;
      
      [Space]
      public float fireRate = 1; //bullet per second 
      private float lastShootTime = Mathf.NegativeInfinity;
      public bool CanShoot()
      {
         if (HaveEnoughBullets() && ReadyToFire())
         {
            bulletsInMagazine--;
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

   public enum WeaponType
   {
      Piston,
      Revolver,
      AutoRifle,
      Shotgun,
      Rifle
   }
}
