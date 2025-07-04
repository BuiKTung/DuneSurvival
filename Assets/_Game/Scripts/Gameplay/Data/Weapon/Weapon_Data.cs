using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Gameplay.Weapon;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System")]
public class Weapon_Data : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;
    public string weaponInfo;
    
    [Header("Bullet info")]
    public int bulletDamage;
    
    [Header("Weapon generics")]
    public WeaponType weaponType;
    [Range(1, 3)]
    public float reloadSpeed = 1;
    [Range(1, 3)]
    public float equipmentSpeed = 1;
    [Range(4, 8)]
    public float gunDistance = 4;
    [Range(4, 8)]
    public float cameraDistance = 6;
    
    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Header("Regular shot")]
    public ShootType shootType;
    public int bulletsPerShot = 1;
    public float fireRate;

    [Header("Burst shot")]
    public bool burstAvalible;
    public bool burstActive;
    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = .1f;

    [Header("Weapon spread")]
    public float baseSpread;
    public float maxSpread;
    public float spreadIncreaseRate = .15f;
}
