using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Gameplay.Weapon;
using UnityEngine;

public enum GrabType { SideGrab, BackGrab };
public enum HoldType {
    CommonHold = 1,
    LowHold = 2,
    HighHold = 3
};
public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    public GrabType grabType;
    public HoldType holdType;
    public Transform gunPoint;
    public Transform holdPoint;
}
