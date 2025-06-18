using _Game.Scripts.Gameplay.Data.Enemy.Enemy_Melee;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Gameplay.Enemy
{
    public class Enemy_WeaponModel : MonoBehaviour
    {
        public Enemy_MeleeWeaponType weaponType;
        public AnimatorOverrideController overrideController;
        public Enemy_MeleeWeaponData weaponData;
        
        [SerializeField] private GameObject[] trailEffects;

        public void EnableTrailEffect(bool enable)
        {
            foreach (var effect in trailEffects)
            {
                effect.SetActive(enable);
            }
        }
    }
}
