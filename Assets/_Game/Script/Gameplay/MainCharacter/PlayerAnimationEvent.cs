using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private PlayerWeaponVisual visualController;
    //private void Start()
    //{
    //    weaponVisualController = GetComponentInParent<WeaponVisualController>();
    //}
    public void ReloadIsOver()
    {
        visualController.MaximizeRigWeight();

        //refill-bullets
    }


    public void ReturnRig()
    {
        visualController.MaximizeRigWeight();
        visualController.MaximizeLeftHandWeight();
    }

    public void WeaponGrabIsOver()
    {
        visualController.SetBusyGrabbingWeaponTo(false);
    }
}
