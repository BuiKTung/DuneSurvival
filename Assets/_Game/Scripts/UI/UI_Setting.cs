using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class UI_Setting : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private float sliderMultiplier = 25;

        [Header("SFX Settings")]
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private TextMeshProUGUI sfxSliderText;
        [SerializeField] private string sfxParametr;

        [Header("BGM Settings")]
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private TextMeshProUGUI bgmSliderText;
        [SerializeField] private string bgmParametr;
        public void SetSFXVolume(float volume)
        {
            sfxSliderText.text = Mathf.RoundToInt(volume * 100) + "%";
            float newValue = Mathf.Log10(volume) * sliderMultiplier;
            audioMixer.SetFloat(sfxParametr, newValue);
        }
        
        public void BGMSliderValue(float volume)
        {
            bgmSliderText.text = Mathf.RoundToInt(volume * 100) + "%";
            float newValue = Mathf.Log10(volume) * sliderMultiplier;
            audioMixer.SetFloat(bgmParametr, newValue);
        }
        public void LoadSettings()
        {
            sfxSlider.value = PlayerPrefs.GetFloat(sfxParametr,.7f);
            bgmSlider.value = PlayerPrefs.GetFloat(bgmParametr,.7f);
        }

        private void OnDisable()
        {
            PlayerPrefs.SetFloat(sfxParametr, sfxSlider.value);
            PlayerPrefs.SetFloat(bgmParametr, bgmSlider.value);
        }
    }
}
