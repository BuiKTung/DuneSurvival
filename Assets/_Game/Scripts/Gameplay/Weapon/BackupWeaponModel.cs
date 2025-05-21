using UnityEngine;

namespace _Game.Scripts.Gameplay.Weapon
{
   public enum HangType
   {
      LowBackHang,
      BackHang,
      SideHang,
   }
   public class BackupWeaponModel : MonoBehaviour
   {
      public WeaponType weaponType;
      [SerializeField] private HangType hangType;
      
      public bool HangTypeIs(HangType hangType) => this.hangType == hangType;  
      public void Activate(bool activated) => gameObject.SetActive(activated);
   }
}
