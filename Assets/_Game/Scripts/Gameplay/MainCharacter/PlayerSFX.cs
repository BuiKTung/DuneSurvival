using System;
using _Game.Scripts.Systems;
using _Game.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Gameplay.MainCharacter
{
    public class PlayerSFX : MonoBehaviour
    {
        [Header("Weapon SFX")]
        public AudioSource weaponAudioSource;
        public AudioClip fireSound;
        public AudioClip reloadSound;
        
        [Header("Movement SFX")]
        public AudioSource movementAudioSource;
        public AudioSource runAudioSource;
        
        private void OnEnable()
        {
            PlayerMovement.OnPlayerMove += HandleMoveSFX;
            PlayerMovement.OnPlayerStop += StopMoveSounds;
            PlayerWeaponController.OnWeaponUsed += HandleWeaponSFX;
        }

        private void OnDisable()
        {
            PlayerMovement.OnPlayerStop -= StopMoveSounds;
            PlayerMovement.OnPlayerMove -= HandleMoveSFX;
            PlayerWeaponController.OnWeaponUsed -= HandleWeaponSFX;
        }

        private void PlayFireSound() => AudioManager.Instance.PlayClipSFX(weaponAudioSource, fireSound);
        private void PlayReloadSound() => AudioManager.Instance.PlayClipSFX(weaponAudioSource, reloadSound);
        private void StopMoveSounds()
        {
            movementAudioSource.Stop();
            runAudioSource.Stop();
        }
        private void PlayWalkSound()
        {
            runAudioSource.Stop();
            AudioManager.Instance.PlayAudioSourceSFX(movementAudioSource);
        }
        private void PlayRunSound()
        {
            movementAudioSource.Stop();
            AudioManager.Instance.PlayAudioSourceSFX(runAudioSource);
        }
        private void HandleWeaponSFX(string action)
        {
            switch (action)
            {
                case ConstantString.AnimationParameter.Fire:
                    PlayFireSound();
                    break;
                case ConstantString.AnimationParameter.Reload:
                    PlayReloadSound();
                    break;
            }
            
        }
        private void HandleMoveSFX(bool isRunning)
        {
            if (isRunning)
            {
                if(runAudioSource.isPlaying) 
                    return;
                PlayRunSound();
            }
            else
            {
                if(movementAudioSource.isPlaying)
                    return;
                PlayWalkSound();
            }
        }
    }
}
