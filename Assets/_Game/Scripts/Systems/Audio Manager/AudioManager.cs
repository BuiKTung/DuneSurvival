using System;
using _Game.Scripts.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Systems
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource[] bgm;
        [SerializeField] private bool playBgm;
        [SerializeField] private int bgmIndex;

        private void Start()
        {
            PlayBGM(bgmIndex);
        }

        private void Update()
        {
            if (playBgm == false && BGMIsPlaying())
                StopAllBGM();


            if (playBgm && bgm[bgmIndex].isPlaying == false)
                PlayRandomBGM();
        }
        public void PlayBGM(int index)
        {
            StopAllBGM();
            
            bgmIndex = index;
            bgm[bgmIndex].Play();
        }

        private void StopAllBGM()
        {
            foreach (var music in bgm)
            {
                music.Stop();
            }
        }
        public void PlayAudioSourceSFX(AudioSource sfx, bool randomPitch = false, float minPitch = .85f, float maxPitch = 1.1f)
        {
            if (sfx == null)
                return;

            float pitch = Random.Range(minPitch, maxPitch);

            sfx.pitch = pitch;
            sfx.Play();
        }
        public void PlayClipSFX(AudioSource sfx,AudioClip clip, bool randomPitch = false, float minPitch = .85f, float maxPitch = 1.1f)
        {
            if (sfx == null)
                return;

            float pitch = Random.Range(minPitch, maxPitch);

            sfx.pitch = pitch;
            sfx.PlayOneShot(clip);
        }
        
        [ContextMenu("Play random bgm")]
        public void PlayRandomBGM()
        {
            StopAllBGM();
            bgmIndex = Random.Range(0, bgm.Length);
            bgm[bgmIndex].Play();
        }
        
        private bool BGMIsPlaying()
        {
            for (int i = 0; i < bgm.Length; i++)
            {
                if (bgm[i].isPlaying) 
                   return true; 
            }
            return false;
        }
    }
}
