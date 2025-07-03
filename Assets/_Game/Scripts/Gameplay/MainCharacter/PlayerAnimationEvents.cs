using _Game.Scripts.Utilities;
using UnityEngine;

namespace _Game.Scripts.Gameplay.MainCharacter
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        private PlayerWeaponVisuals visualController;
        private PlayerWeaponController weaponController;
        private void Start()
        {
            visualController = GetComponentInParent<PlayerWeaponVisuals>();
            weaponController = GetComponentInParent<PlayerWeaponController>();
        }

        public void ReloadIsOver()
        {
            visualController.MaximizeRigWeight();
            //refill-bullets
            weaponController.CurrentWeapon().ReloadBullets();
            weaponController.UpdateWeaponUI();
            weaponController.SetWeaponReady(true);
        }

        public void PlayReloadSound()
        {
            PlayerWeaponController.OnWeaponUsed?.Invoke(ConstantString.AnimationParameter.Reload);
        }

        public void ReturnRig()
        {
            visualController.MaximizeRigWeight();
            visualController.MaximizeLeftHandWeight();
        }

        public void WeaponEquipingIsOver()
        {
            weaponController.SetWeaponReady(true);
        }

        public void SwichOnWeaponModel() => visualController.SwitchOnCurrentWeaponModel();
    }
}
