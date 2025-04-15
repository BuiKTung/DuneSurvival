using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private float currentLayerIndex;
    #region Gun
    [SerializeField] private Transform[] gunTransforms;
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;
    private Transform currentWeapon;
    #endregion
    [Header("Left hand IK")]
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private float leftHandWeightIncreaseStep;
    private bool leftHandWeightShouldBeIncreased;
    private bool busyGrabWeapon;
    [Header("Rig")] 
    [SerializeField] private Rig rig;
    [SerializeField] private float rigIncreaseStep;
    [SerializeField] private bool rigShouldBeIncreased;
    private void Start()
    {
        currentLayerIndex = 0;
        SwitcchOn(pistol); 
        // anim = GetComponentInParent<Animator>(); // Get the animator component from the parent object on/off when it has bug
    }
    private void Update()
    {
        CheckWeaponSwitch();
        if (Input.GetKeyDown(KeyCode.R) && busyGrabWeapon == false)
        {
            Reload();
        }
        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    private void UpdateLeftHandIKWeight()
    {
        if (leftHandWeightShouldBeIncreased)
        {
            leftHandIK.weight += leftHandWeightIncreaseStep * Time.deltaTime;
            if (leftHandIK.weight >= 1)
            {
                leftHandWeightShouldBeIncreased = false;
            }
        }
    }

    private void UpdateRigWeight()
    {
        if (rigShouldBeIncreased)
        {
            rig.weight += rigIncreaseStep * Time.deltaTime;
            if (rig.weight >= 1)
            {
                rigShouldBeIncreased = false;
            }
        }
    }

    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        PauseRig();
        anim.SetFloat(Constant.WeaponGrabType,(float)grabType);
        anim.SetTrigger(Constant.WeaponGrab);
        SetBusyGrabbingWeaponTo(true);
    }
    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        busyGrabWeapon = busy;
        anim.SetBool(Constant.BusyGrabbingWeapon, busyGrabWeapon);
    }
    private void Reload()
    {
        anim.SetTrigger(Constant.Reload);
        PauseRig();
    }

    private void PauseRig()
    {
        rig.weight = 0.15f;
    }

    public void ReturnRigWeigthToOne() => rigShouldBeIncreased = true; //return 1 after reload
    public void ReturnLeftHandIKWeight() => leftHandWeightShouldBeIncreased = true; //return 1 after grab

    private void SwitcchOn(Transform gun)
    {
        SwitchOffGuns();
        gun.gameObject.SetActive(true);
        currentWeapon = gun;
        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        } 
    }
    private void AttachLeftHand()
    {
        Transform targetTransform = currentWeapon.GetComponentInChildren<LeftHandTargetTransform>().transform;
        leftHandIKTarget.localPosition = targetTransform.localPosition;
        leftHandIKTarget.localRotation = targetTransform.localRotation;
    }
    private void SwitchAnimationLayer(int layerIndex)
    {
        if(currentLayerIndex == layerIndex) return;
        for (int i = 1; i < anim.layerCount; i++)
        {
            if(anim.GetLayerWeight(i) > 0)
                anim.SetLayerWeight(i, 0);
        }   
        anim.SetLayerWeight(layerIndex, 1);
        currentLayerIndex = layerIndex;
    }
    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitcchOn(pistol);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitcchOn(revolver);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitcchOn(autoRifle);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitcchOn(shotgun);
            SwitchAnimationLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitcchOn(rifle);
            SwitchAnimationLayer(3);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }
}
