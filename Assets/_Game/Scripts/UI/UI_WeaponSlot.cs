using System.Net.Mime;
using _Game.Scripts.Gameplay.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class UI_WeaponSlot : MonoBehaviour
    {
        public Image weaponIcon;
        public TextMeshProUGUI ammoText;

        private void Awake()
        {
            weaponIcon = GetComponentInChildren<Image>();
            ammoText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void UpdateWeaponSlot(Weapon myWeapon, bool activeWeapon)
        {
            if (myWeapon == null)
            {
                weaponIcon.color = Color.clear;
                ammoText.text = "";
                return;
            }

            Color newColor = activeWeapon ? Color.white : new Color(1, 1, 1, .35f);
            
            weaponIcon.color = newColor;
            weaponIcon.sprite = myWeapon.WeaponData.weaponIcon;

            ammoText.text = myWeapon.bulletsInMagazine + "/" + myWeapon.totalReserveAmmo;
            ammoText.color = Color.white;
        }
    }
}
