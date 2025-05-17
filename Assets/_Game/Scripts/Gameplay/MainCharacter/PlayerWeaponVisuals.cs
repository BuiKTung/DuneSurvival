using _Game.Scripts.Gameplay.Weapon;
using _Game.Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Game.Scripts.Gameplay.MainCharacter
{
    public class PlayerWeaponVisuals : MonoBehaviour
    {
        //private static readonly int WeaponGrabType = Animator.StringToHash("WeaponGrabType");
        //private static readonly int WeaponGrab = Animator.StringToHash("WeaponGrab");
        private Player player;
        private Animator anim;
        private bool isGrabbingWeapon; 

        [SerializeField] private WeaponModel[] weaponModels; 
        [SerializeField] private BackupWeaponModel[] backupWeaponModels;
        
        [Header("Rig ")]
        [SerializeField] private float rigWeightIncreaseRate;
        private bool  shouldIncrease_RigWeight;
        private Rig rig;

        [Header("Left hand IK")]
        [SerializeField] private float leftHandIkWeightIncreaseRate;
        [SerializeField] private TwoBoneIKConstraint leftHandIK;
        [SerializeField] private Transform leftHandIK_Target;
        private bool shouldIncrease_LeftHandIKWieght;



        private void Start()
        {
            player = GetComponent<Player>();
            anim = GetComponentInChildren<Animator>();
            rig = GetComponentInChildren<Rig>();
            weaponModels = GetComponentsInChildren<WeaponModel>(true);
            backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
            //SwitchOn(pistol);
        }

        private void Update()
        {
            UpdateRigWigth();
            UpdateLeftHandIKWeight();
        }

        public WeaponModel CurrentWeaponModel()
        {
            WeaponModel weaponModel = null;
            WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;
            foreach (var weapon in weaponModels )
            {
                if (weapon.weaponType == weaponType)
                    weaponModel = weapon;
            }
            return weaponModel;
        }
        public void PlayReloadAnimation()
        {
            if(isGrabbingWeapon)
                return;
            anim.SetTrigger(Constant.Reload);
            ReduceRigWeight();
        }

        
        public void PlayWeaponEquipAnimation()
        {
            GrabType grabType = CurrentWeaponModel().grabType;
            
            leftHandIK.weight = 0;
            ReduceRigWeight();
            anim.SetFloat(Constant.WeaponGrabType, ((float)grabType));
            anim.SetTrigger(Constant.WeaponGrab);

            SetBusyGrabbingWeaponTo(true);
        }
        public void SetBusyGrabbingWeaponTo(bool busy)
        {
            isGrabbingWeapon = busy;
            anim.SetBool(Constant.BusyGrabbingWeapon, isGrabbingWeapon);
        }
        public void SwitchOnCurrentWeaponModel()
        {
            int animationIndex = ((int)CurrentWeaponModel().holdType);
            
            SwitchOffWeaponModels();
            
            SwitchOffBackupWeaponModels();
            SwitchOnBackupWeaponModel();
            
            if(player.weapon.HasOnlyOneWeapon() == false)
                SwitchOnBackupWeaponModel();
            
            SwitchAnimationLayer(animationIndex);
            CurrentWeaponModel().gameObject.SetActive(true);
            AttachLeftHand();
        }
        public void SwitchOffWeaponModels()
        {
            for (int i = 0; i < weaponModels.Length; i++)
            {
                weaponModels[i].gameObject.SetActive(false);
            }
        }

        private void SwitchOffBackupWeaponModels()
        {
            foreach (BackupWeaponModel backupModel in backupWeaponModels)
            {
                backupModel.gameObject.SetActive(false);
            }
        }

        public void SwitchOnBackupWeaponModel()
        {
            if(player.weapon.BackupWeapon() == null) return;
            WeaponType weaponType = player.weapon.BackupWeapon().weaponType;
            
            foreach (BackupWeaponModel backupModel in backupWeaponModels)
            {
                if(backupModel.weaponType == weaponType)
                    backupModel.gameObject.SetActive(true); 
            }
        }
        private void SwitchAnimationLayer(int layerIndex)
        {
            for (int i = 1; i < anim.layerCount; i++)
            {
                anim.SetLayerWeight(i, 0);
            }

            anim.SetLayerWeight(layerIndex, 1);
        }
        #region Animation Rigging Methods
        private void AttachLeftHand()
        {
            Transform targetTransform = CurrentWeaponModel().holdPoint;

            leftHandIK_Target.localPosition = targetTransform.localPosition;
            leftHandIK_Target.localRotation = targetTransform.localRotation;
        }
        private void UpdateLeftHandIKWeight()
        {
            if (shouldIncrease_LeftHandIKWieght)
            {
                leftHandIK.weight += leftHandIkWeightIncreaseRate * Time.deltaTime;

                if (leftHandIK.weight >= 1)
                    shouldIncrease_LeftHandIKWieght = false;
            }
        }
        private void UpdateRigWigth()
        {
            if (shouldIncrease_RigWeight)
            {
                rig.weight += rigWeightIncreaseRate * Time.deltaTime;

                if (rig.weight >= 1)
                    shouldIncrease_RigWeight = false;
            }
        }
        private void ReduceRigWeight()
        {
            rig.weight = .15f;
        }
        public void MaximizeRigWeight() => shouldIncrease_RigWeight = true;
        public void MaximizeLeftHandWeight() => shouldIncrease_LeftHandIKWieght = true;
        #endregion
    }
}
