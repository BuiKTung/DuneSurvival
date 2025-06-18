using System.Collections.Generic;
using _Game.Scripts.Gameplay.Enemy;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Data.Enemy.Enemy_Melee
{
    [CreateAssetMenu(fileName = "New Weapon Data", menuName = "Enemy data/Melee weapon Data")]
    public class Enemy_MeleeWeaponData : ScriptableObject
    {
        public List<AttackData_EnemyMelee> attackData;
        public float turnSpeed = 10;
    }
}
