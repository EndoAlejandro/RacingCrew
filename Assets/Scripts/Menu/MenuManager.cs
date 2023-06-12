using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Menu {
	public class MenuManager : MonoBehaviour
	{
		[Header("SETTINGS"), Space(3)]
		[SerializeField] GameObject _screenMainMenu;
		[SerializeField] GameObject _screenSelectMode;
		[SerializeField] GameObject _screenSelectCup;
		[SerializeField] GameObject _screenLobby;
		[SerializeField] GameObject _screenSettings;
		[SerializeField] GameObject _screenCredits;
		[Space(5)]
		[Header("Audio")]
		[SerializeField] AudioMixer audioMixer;
		[SerializeField] Slider sliderMusicVolume;
		[SerializeField] Slider sliderSoundFXVolume;
		[SerializeField] Slider sliderGlobalVolume;
		[Space(5)]
		[Header("Language")]
		[SerializeField] TextMeshProUGUI _languageText;
		[Space(5)]
		[Header("Resolution")]
		[SerializeField] TMP_Dropdown resolutionDropdown;
		[SerializeField] Toggle fullscreenToggle;




		//LANGUAGE
		private int _currentLocaleIndex;

		private void Awake()
		{
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

		public void OnBack() {
			if (_screenSelectMode.activeSelf) { 
				_screenSelectMode.SetActive(false);
				_screenMainMenu.SetActive(true);
			}

			if (_screenSelectCup.activeSelf) {
				_screenSelectCup.SetActive(false);
				_screenSelectMode.SetActive(true);
			}

			if (_screenLobby.activeSelf) { 
				_screenLobby.SetActive(false);
				_screenSelectCup.SetActive(true);
			}

			if (_screenSettings.activeSelf) { 
				_screenSettings.SetActive(false);
				_screenMainMenu.SetActive(true);
			}

			if (_screenCredits.activeSelf) {
				_screenCredits.SetActive(false);
				_screenMainMenu.SetActive(true);
			}
		}

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
					_languageText.text = "English";
					break;
				case 1:
					_languageText.text = "Español";
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



