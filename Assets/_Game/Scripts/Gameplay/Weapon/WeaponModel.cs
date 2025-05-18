using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Gameplay.Weapon;
using UnityEngine;
using UnityEngine.Serialization;

public enum EquipType { SideEquipAnimation, BackEquipAnimation };
public enum HoldType {
    CommonHold = 1,
    LowHold = 2,
    HighHold = 3
};
public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    public EquipType equipType;
    public HoldType holdType;
    public Transform gunPoint;
    public Transform holdPoint;
}
