using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using CustomUtils;

namespace Menu
{
    public class MenuManager : Singleton<MenuManager>
    {
        [Space(5)]
        [Header("Audio")]
        [SerializeField] private AudioMixer audioMixer;

        [SerializeField] private Slider sliderMusicVolume;
        [SerializeField] private Slider sliderSoundFXVolume;
        [SerializeField] private Slider sliderGlobalVolume;

        [Space(5)]
        [Header("Language")]
        [SerializeField] private TextMeshProUGUI languageText;

        [Space(5)]
        [Header("Resolution")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;

        [SerializeField] private Toggle fullscreenToggle;

        // LANGUAGE
        private int _currentLocaleIndex;
        private int _selectedCupIndex;


        protected override void Awake()
        {
            base.Awake();
            VolumeSettings();
            LocalizationCheck();
            ScreenConfiguration();
        }

        private void ScreenConfiguration()
        {
            resolutionDropdown.options.Clear();
            for (int i = 0; i < Screen.resolutions.Length; ++i)
            {
                resolutionDropdown.options.Add(
                    new TMP_Dropdown.OptionData(Screen.resolutions[i].width + " x " + Screen.resolutions[i].height));

                if (Screen.width == Screen.resolutions[i].width && Screen.height == Screen.resolutions[i].height)
                {
                    resolutionDropdown.value = i;
                }
            }

            if (fullscreenToggle.isOn)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            }
        }

        private static void LocalizationCheck()
        {
            if (!PlayerPrefs.HasKey("Language")) PlayerPrefs.SetInt("Language", 0);
        }

        private void VolumeSettings()
        {
            if (!PlayerPrefs.HasKey("MusicVolume")) PlayerPrefs.SetFloat("MusicVolume", 1);

            OnMusicVolumeChange(PlayerPrefs.GetFloat("MusicVolume"));
            sliderMusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");

            if (!PlayerPrefs.HasKey("SoundFXVolume")) PlayerPrefs.SetFloat("SoundFXVolume", 1);

            OnSoundFXVolumeChange(PlayerPrefs.GetFloat("SoundFXVolume"));
            sliderSoundFXVolume.value = PlayerPrefs.GetFloat("SoundFXVolume");

            if (!PlayerPrefs.HasKey("GlobalVolume")) PlayerPrefs.SetFloat("GlobalVolume", 1);

            OnGlobalVolumeChange(PlayerPrefs.GetFloat("GlobalVolume"));
            sliderGlobalVolume.value = PlayerPrefs.GetFloat("GlobalVolume");
        }

        IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            _currentLocaleIndex = PlayerPrefs.GetInt("Language");
            UpdateLocale();
        }

        public void NextLocale()
        {
            _currentLocaleIndex = (_currentLocaleIndex + 1) % LocalizationSettings.AvailableLocales.Locales.Count;
            UpdateLocale();
        }

        public void PrevLocale()
        {
            _currentLocaleIndex = (_currentLocaleIndex - 1 + LocalizationSettings.AvailableLocales.Locales.Count) %
                                  LocalizationSettings.AvailableLocales.Locales.Count;
            UpdateLocale();
        }

        public void UpdateLocale()
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_currentLocaleIndex];
            PlayerPrefs.SetInt("Language", _currentLocaleIndex);
            UpdateUILanguage();
        }

        public void UpdateUILanguage()
        {
            switch (_currentLocaleIndex)
            {
                case 0:
                    languageText.text = "English";
                    break;
                case 1:
                    languageText.text = "Espa�ol";
                    break;
            }
        }

        public void OnMusicVolumeChange(float value)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log(value) * 20);
            PlayerPrefs.SetFloat("MusicVolume", value);
        }

        public void OnSoundFXVolumeChange(float value)
        {
            audioMixer.SetFloat("SoundFXVolume", Mathf.Log(value) * 20);
            PlayerPrefs.SetFloat("SoundFXVolume", value);
        }

        public void OnGlobalVolumeChange(float value)
        {
            audioMixer.SetFloat("GlobalVolume", Mathf.Log(value) * 20);
            PlayerPrefs.SetFloat("GlobalVolume", value);
        }

        public void PlaySoundFX() => SoundManager.Instance.PlayFx(Sfx.UI);

        public void UpdateScreen()
        {
            int width = Screen.resolutions[resolutionDropdown.value].width;
            int height = Screen.resolutions[resolutionDropdown.value].height;
            Screen.SetResolution(width, height,
                fullscreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
        }

        public void QuitApplication() => Application.Quit();
    }
}