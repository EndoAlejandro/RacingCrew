using Cinemachine;
using System.Collections;
using UnityEngine;

public class CarCameraEffects : MonoBehaviour
{
	[Header("GENERAL")]
	[SerializeField] CinemachineVirtualCamera virtualCamera;
	[SerializeField] private float speedToStartCameraEffects;


	[Header("NOISE SETTINGS")]
	[SerializeField] private float maxAmplitudeGain;
	[SerializeField] private float transitionSpeedNoise;

	[Header("CAMERA SETTINGS")]
	[SerializeField] private Vector3 minFollowOffset;
	[SerializeField] private Vector3 maxFollowOffset;
	[SerializeField] private float transitionSpeedCamera;

	[Header("FIELD OF VIEW")]
	[SerializeField] private float minFieldOfView;
	[SerializeField] private float maxFieldOfView;
	[SerializeField] private float transitionSpeedFOV;



	private Rigidbody _carRigidbody;
	private float _tForNoise;
	private float _tForOffset;
	private float _tForFOV;
	private float _fovProgress;
	private bool _back = false;

	private void Awake()
	{
		_carRigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (_carRigidbody.velocity.magnitude > speedToStartCameraEffects)
		{
			AddNoise();
			AddCameraOffset();
			AddCameraFOV();	
			_back = false;
		}
		else
		{
			if (!_back) {
				_back = true;
			}
			RemoveNoise();
			RemoveCameraOffset();
			RemoveCameraFOV();

		}
	}

	public void AddNoise() {
		if (_tForNoise <= 1) {
			_tForNoise += Time.deltaTime * transitionSpeedNoise;
		}	
		virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(0, maxAmplitudeGain, _tForNoise);
	}

	public void RemoveNoise()
	{
		if (virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain != 0)
		{
			if (_tForNoise >= 0) {
				_tForNoise -= Time.deltaTime * transitionSpeedNoise;
			}		
			virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(0, maxAmplitudeGain, _tForNoise);
			if (virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain == 0)
			{
				_tForNoise = 0;
			}
		}
	}

	public void AddCameraOffset() {
		if (_tForOffset <= 1) {
			_tForOffset += Time.deltaTime * transitionSpeedCamera;
		}	
		virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(maxFollowOffset, minFollowOffset, _tForOffset);
	}

	public void RemoveCameraOffset()
	{
		if (virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset != maxFollowOffset)
		{
			if (_tForOffset >= 0) {
				_tForOffset -= Time.deltaTime * transitionSpeedCamera;
			}			
			virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(maxFollowOffset, minFollowOffset, _tForOffset);
			if (virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset == maxFollowOffset)
			{
				_tForOffset = 0;
			}
		}

	}

	public void AddCameraFOV() {
		if (_tForFOV <= 1) {
			_tForFOV += Time.deltaTime * transitionSpeedFOV;
		}		
		virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(maxFieldOfView, minFieldOfView, _tForFOV);
	}

	public void RemoveCameraFOV()
	{
		if (virtualCamera.m_Lens.FieldOfView != maxFieldOfView)
		{
			if (_tForFOV >= 0) {
				_tForFOV -= Time.deltaTime * transitionSpeedFOV;
			}		
			virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(maxFieldOfView, minFieldOfView, _tForFOV);
			if (virtualCamera.m_Lens.FieldOfView == maxFieldOfView)
			{
				_tForFOV = 0;
			}
		}

	}
}
