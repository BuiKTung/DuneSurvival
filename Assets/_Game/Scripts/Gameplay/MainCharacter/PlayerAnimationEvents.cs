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
            weaponController.SetWeaponReady(true);
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
