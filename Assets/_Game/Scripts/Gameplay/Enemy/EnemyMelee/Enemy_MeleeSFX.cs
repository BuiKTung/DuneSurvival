using System;
using _Game.Scripts.Systems;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy.EnemyMelee
{
    public class Enemy_MeleeSFX : MonoBehaviour
    {
        [SerializeField] private AudioSource attackAudioSource;
        [SerializeField] private AudioClip swooshSFX;
        [SerializeField] private AudioClip roarSFX;
        [SerializeField] private Enemy_AnimationEvents animationEvents;
        

        private void PlayRoarSFX() => AudioManager.Instance.PlayClipSFX(attackAudioSource, roarSFX);
        private void PlaySwooshSFX() => AudioManager.Instance.PlayClipSFX(attackAudioSource,swooshSFX,true);

        private void OnEnable()
        {
           animationEvents.OnAttackTriggered += PlaySwooshSFX;
           animationEvents.OnRoarTriggered += PlayRoarSFX;
        }

        private void OnDisable()
        {
            animationEvents.OnAttackTriggered -= PlaySwooshSFX;
            animationEvents.OnRoarTriggered -= PlayRoarSFX;
        }
    }
}
