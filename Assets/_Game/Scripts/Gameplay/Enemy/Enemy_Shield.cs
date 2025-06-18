using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Gameplay.Enemy;
using _Game.Scripts.Utilities;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Constant = _Game.Scripts.Utilities.Constant;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private int durability = 10;
    [SerializeField] private Enemy_Melee enemy;

    public int CurrentDurability() => durability;
    public void ReduceDurability()
    {
        durability--;
        if (durability <= 0)
        {
            enemy.anim.SetFloat(Constant.AnimationParameter.ChaseIndex, 0);
            CacheComponent<EnemyShield>.ClearCaches(gameObject);
            gameObject.SetActive(false);
        }
    }
}
            