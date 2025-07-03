using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class UI_WeaponSelectionButton : UI_Button
    {
        [SerializeField] private Weapon_Data weaponData;
        [SerializeField] private Image weaponIcon;
        
        private UI_WeaponSelection weaponSelectionUI;
        private UI_SelectedWeaponWindow emptySlot;

        private void OnValidate()
        {
            gameObject.name = "Button - Select Weapon: " + weaponData.weaponType;
        }
        public override void Start()
        {
            base.Start();

            weaponSelectionUI = GetComponentInParent<UI_WeaponSelection>();
            weaponIcon.sprite = weaponData.weaponIcon;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            weaponIcon.color = Color.yellow;

            emptySlot = weaponSelectionUI.FindEmptySlot();
            emptySlot?.UpdateSlotInfo(weaponData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            weaponIcon.color = Color.white;
            
            emptySlot?.UpdateSlotInfo(null);
            emptySlot = null;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            weaponIcon.color = Color.white;
            
            bool noMoreEmptySlot = weaponSelectionUI.FindEmptySlot() == null;
            UI_SelectedWeaponWindow slotBusyWithThisWeapon = weaponSelectionUI.FindWeaponTypeInSlot(weaponData);
            bool noThisWeaponInSlots = slotBusyWithThisWeapon == null;

            if (noMoreEmptySlot && noThisWeaponInSlots)
            {
                weaponSelectionUI.ShowWarningMessage("No more empty slots available....");
                return;
            }
            
            if (slotBusyWithThisWeapon != null)
            {
                slotBusyWithThisWeapon.SetWeaponSlot(null);
            }
            else
            {
                emptySlot = weaponSelectionUI.FindEmptySlot();
                emptySlot.SetWeaponSlot(weaponData);
            }
            
            emptySlot = null;
        }
    }
}
