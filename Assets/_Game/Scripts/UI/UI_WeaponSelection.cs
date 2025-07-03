using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI
{
    public class UI_WeaponSelection : MonoBehaviour
    {
        public UI_SelectedWeaponWindow[] selectedWeapon;
        [SerializeField] private GameObject nextUIToSwitchOn;

        [Header("Warning Info")]
        [SerializeField] private TextMeshProUGUI warningText;
        [SerializeField] private float disaperaingSpeed = .25f;
        private float currentWarningAlpha;
        private float targetWarningAlpha;

        private void Update()
        {
            if (currentWarningAlpha > targetWarningAlpha)
            {
                currentWarningAlpha -= Time.deltaTime * disaperaingSpeed;
                warningText.color = new Color(1, 1, 1, currentWarningAlpha);
            }
        }
        private bool AtLeastOneWeaponSelected() => SelectedWeaponData().Count > 0;
        public List<Weapon_Data> SelectedWeaponData()
        {
            List<Weapon_Data> selectedData = new List<Weapon_Data> ();

            foreach(UI_SelectedWeaponWindow weapon in selectedWeapon)
            {
                if(weapon.weaponData != null)
                    selectedData.Add(weapon.weaponData);
            }

            return selectedData;
        }
        public UI_SelectedWeaponWindow FindEmptySlot()
        {
            for (int i = 0; i < selectedWeapon.Length; i++)
            {
                if (selectedWeapon[i].IsEmpty())
                {
                    return selectedWeapon[i];
                }
            }
            return null;
        }

        public UI_SelectedWeaponWindow FindWeaponTypeInSlot(Weapon_Data weaponData)
        {
            for(int i = 0;i < selectedWeapon.Length;i++)
            {
                if (selectedWeapon[i].weaponData == weaponData)
                    return selectedWeapon[i];
            }
            return null;
        }
        public bool ConfirmWeaponSelection()
        {
            if (AtLeastOneWeaponSelected())
            {
                //UI.Instance.SwitchTo(nextUIToSwitchOn);
                //UI.Instance.StartLevelGeneration();
                return true;
            }
            else
            {
                ShowWarningMessage("Select at least one weapon.");
                return false;
            }
        }
        public void ShowWarningMessage(string message)
        {
            warningText.color = Color.white;
            warningText.text = message;

            currentWarningAlpha = warningText.color.a;
            targetWarningAlpha = 0;
        }
    }
}
