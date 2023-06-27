using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUtils;

namespace InGame {
	public class PlayerCameraSettings : Singleton<PlayerCameraSettings>
	{

		[SerializeField] private CinemachineVirtualCamera[] cameras;
		[SerializeField] private GameObject target;

		private int _currentCameraIndex = 0;

		protected override void Awake()
		{
			base.Awake();
			SetTarget(GameObject.FindGameObjectWithTag("Player"));
		}

		public void OnNextOption() {
			ChangeNextCamera();
		}

		public void OnPrevOption() { 
			ChangePrevCamera();
		}

		public void ChangeNextCamera() {
			cameras[_currentCameraIndex].Priority = 1;
			_currentCameraIndex++;
			if (_currentCameraIndex > cameras.Length-1) {
				_currentCameraIndex = 0;
			}
			cameras[_currentCameraIndex].Priority = 10;
		}

		public void ChangePrevCamera() {
			cameras[_currentCameraIndex].Priority = 1;
			_currentCameraIndex--;
			if (_currentCameraIndex < 0)
			{
				_currentCameraIndex = cameras.Length-1;
			}
			cameras[_currentCameraIndex].Priority = 10;
		}

		public void SetTarget(GameObject targetToFollow) {
			for (int i = 0; i < cameras.Length;i++) {
				cameras[i].LookAt = targetToFollow.transform;
				cameras[i].Follow = targetToFollow.transform;
				
			}
		}

	}
}

