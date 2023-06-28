using CarComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParticles : MonoBehaviour
{
	[Header("PARTICLE SYSTEM AIR")]
	[SerializeField] private ParticleSystem airParticleSystemFront;
	[SerializeField] private ParticleSystem airParticleSystemBack;
	[SerializeField] private float _speedToStartAirParticleSystem;
	[Header("CAR")]
	[SerializeField] private GameObject car;


	private Rigidbody _carRigidbody;
	private float _dotProductResult;

	private void Awake()
	{
		_carRigidbody = GetComponent<Rigidbody>();	
	}

	private void Update()
	{
		_dotProductResult = Vector3.Dot(car.transform.forward.normalized, _carRigidbody.velocity.normalized);
		if (_dotProductResult > 0)
		{
			ActivateAirParticleSystemFront();
		}
		else {
			ActivateAirParticleSystemBack();
		}
	}

	public void ActivateAirParticleSystemFront() {
		if (_carRigidbody.velocity.magnitude > _speedToStartAirParticleSystem)
		{
			if (!airParticleSystemFront.isPlaying)
			{
				airParticleSystemFront.Play();
			}
		}
		else
		{
			airParticleSystemFront.Stop();
		}
	}

	public void ActivateAirParticleSystemBack() {
		if (_carRigidbody.velocity.magnitude > _speedToStartAirParticleSystem)
		{
			if (!airParticleSystemBack.isPlaying)
			{
				airParticleSystemBack.Play();
			}
		}
		else
		{
			airParticleSystemBack.Stop();
		}
	}

}
