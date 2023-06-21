using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Menu {
	public class MenuManager : MonoBehaviour
	{
		public static MenuManager Instance;

		[Header("PLAYER INPUT MANAGER")]
		[SerializeField] PlayerInputManager playerInputManager;

		[Header("COPAS")]
		[SerializeField] AllCupsAssets allCupsAssets;

		[Header("SETTINGS"), Space(3)]
		[SerializeField] GameObject screenMainMenu;
		[SerializeField] GameObject screenSelectMode;
		[SerializeField] GameObject screenSelectCup;
		[SerializeField] GameObject screenLobby;
		[SerializeField] GameObject screenSettings;
		[SerializeField] GameObject screenCredits;
		[Space(5)]
		[Header("Audio")]
		[SerializeField] AudioMixer audioMixer;
		[SerializeField] Slider sliderMusicVolume;
		[SerializeField] Slider sliderSoundFXVolume;
		[SerializeField] Slider sliderGlobalVolume;
		[Space(5)]
		[Header("Language")]
		[SerializeField] TextMeshProUGUI languageText;
		[Space(5)]
		[Header("Resolution")]
		[SerializeField] TMP_Dropdown resolutionDropdown;
		[SerializeField] Toggle fullscreenToggle;


		//Sound FX
		private AudioSource _audioSource;

		//LANGUAGE
		private int _currentLocaleIndex;

		//CUPS
		private int _selectedCupIndex;

		//Player ready
		private int _playersReady;

		public int PlayersReady {
			get { return _playersReady; }
			set { 
				_playersReady = value;
				Debug.Log("Current Number of Players: " + _playersReady);
				if (_playersReady == playerInputManager.playerCount) {
					ChangeScene();
				}
			}
		}

		public int SelectedCupIndex { set { _selectedCupIndex = value; } }


		private void Awake()
		{
			Instance= this;

			_audioSource = GameObject.FindGameObjectWithTag("SoundFX").GetComponent<AudioSource>();

			//Comprueba si hay información guardada sobre volumen, si no hay crea info por defecto 
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

			if (!PlayerPrefs.HasKey("Language")) {
				PlayerPrefs.SetInt("Language", 0);
			}

			//Personaliza el dropdown con las opciones disponibles
			resolutionDropdown.options.Clear();
			for (int i = 0; i < Screen.resolutions.Length; ++i) {
				resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(Screen.resolutions[i].width + " x " + Screen.resolutions[i].height));

				if (Screen.width == Screen.resolutions[i].width && Screen.height == Screen.resolutions[i].height) {
					resolutionDropdown.value = i;
				}
			}

			if (fullscreenToggle.isOn) {
				Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
			}
		}

		IEnumerator Start()
		{
			yield return LocalizationSettings.InitializationOperation;
			_currentLocaleIndex = PlayerPrefs.GetInt("Language");
			UpdateLocale();
		}

		public void ChangeScene() {
			int index = 0;

			for (int i = 0; i < allCupsAssets.cups.Length;i++) {
				if (allCupsAssets.cups[i].cupID == PlayerPrefs.GetInt("CurrentCupID")) { 
					index= i;
					break;
				}
			}

			PlayerPrefs.SetInt("CurrentSpeedway",0);
			PlayerPrefs.SetInt("NumberOfPlayers", playerInputManager.playerCount);
			SceneManager.LoadScene(allCupsAssets.cups[index].speedwayNames[PlayerPrefs.GetInt("CurrentSpeedway")]);

		}

		#region INPUT SYSTEM
		public void OnBack() {
			if (screenSelectMode.activeSelf) { 
				screenSelectMode.SetActive(false);
				screenMainMenu.SetActive(true);
			}

			if (screenSelectCup.activeSelf) {
				screenSelectCup.SetActive(false);
				screenSelectMode.SetActive(true);
			}

			if (screenLobby.activeSelf) { 
				screenLobby.SetActive(false);
				screenSelectCup.SetActive(true);
			}

			if (screenSettings.activeSelf) { 
				screenSettings.SetActive(false);
				screenMainMenu.SetActive(true);
			}

			if (screenCredits.activeSelf) {
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
			_currentLocaleIndex = (_currentLocaleIndex - 1 + LocalizationSettings.AvailableLocales.Locales.Count) % LocalizationSettings.AvailableLocales.Locales.Count;
			UpdateLocale();
		}

		public void UpdateLocale() {
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
					languageText.text = "Español";
					break;
			}
		}
		#endregion

		#region SOUND SETTINGS
		public void OnMusicVolumeChange(float value) {
			audioMixer.SetFloat("MusicVolume", Mathf.Log(value) * 20);
			PlayerPrefs.SetFloat("MusicVolume", value);
		}

		public void OnSoundFXVolumeChange(float value) {
			audioMixer.SetFloat("SoundFXVolume", Mathf.Log(value) * 20);
			PlayerPrefs.SetFloat("SoundFXVolume", value);
		}

		public void OnGlobalVolumeChange(float value)
		{
			audioMixer.SetFloat("GlobalVolume", Mathf.Log(value) * 20);
			PlayerPrefs.SetFloat("GlobalVolume", value);
		}

		public void PlaySoundFX() {
			SoundManager.Instance.PlayFx(Sfx.UI);
		}

		#endregion

		#region RESOLUTION
		public void UpdateScreen()
		{
			int width = Screen.resolutions[resolutionDropdown.value].width;
			int height = Screen.resolutions[resolutionDropdown.value].height;
			Screen.SetResolution(width, height,fullscreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
		}
		#endregion

		#region QUIT SETTINGS
		public void QuitApplication() { 
			Application.Quit();
		}
		#endregion
	}
}



