using System.Collections.Generic;
using _Game.Scripts.Gameplay.MainCharacter;
using _Game.Scripts.Gameplay.Weapon;
using _Game.Scripts.Utilities;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Interactables
{
    public enum AmmoBoxType
    {
        SmallBox,
        LargeBox
    }
    [System.Serializable]
    public struct AmmoData
    {
        public WeaponType weaponType;
        [Range(10, 40)] public int minAmount;
        [Range(41, 100)] public int maxAmount;
    }
    public class PickupAmmo : Interactable
    {

        
        [SerializeField] private AmmoBoxType boxType;
        [SerializeField]  private List<AmmoData> smallBoxAmmo;
        [SerializeField]  private List<AmmoData> largeBoxAmmo;
        
        [SerializeField]  private GameObject[] boxModel;

        private void Start()
        {
            SetupBoxModel();
        }
        [ContextMenu("Setup Box Model")]
        private void SetupBoxModel()
        {
            for (int i = 0; i < boxModel.Length; i++)
            {
                boxModel[i].SetActive(false);
                if (i == (int)boxType)
                {
                    boxModel[i].SetActive(true);
                    UpdateMeshAndMaterial(boxModel[i].GetComponent<MeshRenderer>());
                }
            }
        }

        public override void Interaction()
        {
            List<AmmoData> currentAmmoList = smallBoxAmmo;
            if(boxType == AmmoBoxType.LargeBox)
                currentAmmoList = largeBoxAmmo;

            foreach (AmmoData ammo in currentAmmoList)
            {
                Weapon.Weapon weapon = weaponController.WeaponInSlots(ammo.weaponType);
                AddBulletToWeapon(weapon, GetBulletAmount(ammo));
            }
            
            ObjectPool.Instance.ReturnToPool(gameObject);
        }

        private int GetBulletAmount(AmmoData ammo)
        {
            return Mathf.RoundToInt(Random.Range(ammo.minAmount, ammo.maxAmount));
        }
        private void AddBulletToWeapon(Weapon.Weapon weapon, int amount)
        {
            if (weapon == null)
            {
                return;
            }
            weapon.totalReserveAmmo += amount;
        }
    }
}
