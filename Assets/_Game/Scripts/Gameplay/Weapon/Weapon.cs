using UnityEngine.Serialization;

namespace _Game.Scripts.Gameplay.Weapon
{
   [System.Serializable]
   public class Weapon 
   {
      public WeaponType type;
      [FormerlySerializedAs("ammo")] public int bulletsInMagazine;
      public int magazineCapacity;
      [FormerlySerializedAs("maxAmmo")] public int totalReserveAmmo;
      public bool CanShoot()
      {
         return HaveEnoughBullets();
      }

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
      private bool HaveEnoughBullets()
      {
         if (bulletsInMagazine > 0)
         {
            bulletsInMagazine--;
            return true;
         }

         return false;
      }
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
