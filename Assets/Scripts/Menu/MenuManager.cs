using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using CustomUtils;
using InGame;

namespace Menu
{
    public class MenuManager : Singleton<MenuManager>
    {
        [Header("PLAYER INPUT MANAGER")]
        [SerializeField] private PlayerInputManager playerInputManager;

        [Header("COPAS")]
        [SerializeField] private AllCupsAssets allCupsAssets;

        [Header("SETTINGS"), Space(3)]
        [SerializeField] private GameObject screenMainMenu;

        [SerializeField] private GameObject screenSelectMode;
        [SerializeField] private GameObject screenSelectCup;
        [SerializeField] private GameObject screenLobby;
        [SerializeField] private GameObject screenSettings;
        [SerializeField] private GameObject screenCredits;

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

        //LANGUAGE
        private int _currentLocaleIndex;

        //CUPS
        private int _selectedCupIndex;

        //Player ready
        private int _playersReady;

        public int PlayersReady
        {
            get { return _playersReady; }
            set
            {
                _playersReady = value;
                CheckNumberOfPlayers();
            }
        }

        public int SelectedCupIndex
        {
            set { _selectedCupIndex = value; }
        }

        protected override void Awake()
        {
            base.Awake();

            //Comprueba si hay informaci�n guardada sobre volumen, si no hay crea info por defecto 
            if (!PlayerPrefs.HasKey("MusicVolume"))
            {
                PlayerPrefs.SetFloat("MusicVolume", 1);
            }

            OnMusicVolumeChange(PlayerPrefs.GetFloat("MusicVolume"));
            sliderMusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");

            if (!PlayerPrefs.HasKey("SoundFXVolume"))
            {
                PlayerPrefs.SetFloat("SoundFXVolume", 1);
            }

            OnSoundFXVolumeChange(PlayerPrefs.GetFloat("SoundFXVolume"));
            sliderSoundFXVolume.value = PlayerPrefs.GetFloat("SoundFXVolume");

            if (!PlayerPrefs.HasKey("GlobalVolume"))
            {
                PlayerPrefs.SetFloat("GlobalVolume", 1);
            }

            OnGlobalVolumeChange(PlayerPrefs.GetFloat("GlobalVolume"));
            sliderGlobalVolume.value = PlayerPrefs.GetFloat("GlobalVolume");

            if (!PlayerPrefs.HasKey("Language"))
            {
                PlayerPrefs.SetInt("Language", 0);
            }

            //Personaliza el dropdown con las opciones disponibles
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

        IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            _currentLocaleIndex = PlayerPrefs.GetInt("Language");
            UpdateLocale();
        }

        public void CheckNumberOfPlayers()
        {
            if (_playersReady == playerInputManager.playerCount)
            {
                ChangeScene();
            }
        }

        private void ChangeScene()
        {
            // GameManager.Instance.SetPlayersCount(playerInputManager.playerCount);
            GameManager.Instance.StartCup();
        }

        #region INPUT SYSTEM

        public void OnBack()
        {
            if (screenSelectMode.activeSelf)
            {
                screenSelectMode.SetActive(false);
                screenMainMenu.SetActive(true);
            }

            if (screenSelectCup.activeSelf)
            {
                screenSelectCup.SetActive(false);
                screenSelectMode.SetActive(true);
            }

            if (screenLobby.activeSelf)
            {
                screenLobby.SetActive(false);
                screenSelectCup.SetActive(true);
            }

            if (screenSettings.activeSelf)
            {
                screenSettings.SetActive(false);
                screenMainMenu.SetActive(true);
            }

            if (screenCredits.activeSelf)
            {
                screenCredits.SetActive(false);
                screenMainMenu.SetActive(true);
            }
        }

        #endregion

        #region LANGUAGE

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

        #endregion

        #region SOUND SETTINGS

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

        public void PlaySoundFX()
        {
            SoundManager.Instance.PlayFx(Sfx.UI);
        }

        #endregion

        #region RESOLUTION

        public void UpdateScreen()
        {
            int width = Screen.resolutions[resolutionDropdown.value].width;
            int height = Screen.resolutions[resolutionDropdown.value].height;
            Screen.SetResolution(width, height,
                fullscreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
        }

        #endregion

        #region QUIT SETTINGS

        public void QuitApplication()
        {
            Application.Quit();
        }

        #endregion
    }
}