using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundFX : MonoBehaviour
{
	[SerializeField] private CarSounds carSounds;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private float _speedVolumeTransitionUp;
	[SerializeField] private float _speedVolumeTransitionDown;


	private Rigidbody _carRigidbody;
	private float _tForVolume;

	private void Awake()
	{
		_carRigidbody = GetComponent<Rigidbody>();
	}
	private void Update()
	{

		Debug.Log(_carRigidbody.velocity.magnitude);
		if (_carRigidbody.velocity.magnitude > 5)
		{
			if (_tForVolume <= 1)
			{
				_tForVolume += Time.deltaTime * _speedVolumeTransitionUp;
			}
			audioSource.volume = Mathf.Lerp(0, 1, _tForVolume);
		}
		
		if(_carRigidbody.velocity.magnitude < 20){
			if (_tForVolume >= 0) { 
				_tForVolume -= Time.deltaTime * _speedVolumeTransitionDown;
			}
			audioSource.volume = Mathf.Lerp(0, 1, _tForVolume);
		}
		
	}
}
