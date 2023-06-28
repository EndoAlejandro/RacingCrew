using CarComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLights : MonoBehaviour
{
	[Header("CAR")]
	[SerializeField] private GameObject car;
	[Header("LIGHTS")]
	[SerializeField] private GameObject backLights;

	private Rigidbody _carRigidbody;
	private float _dotProductResult;

	private void Awake()
	{
		_carRigidbody = GetComponent<Rigidbody>();
	}
	private void Update()
	{
		_dotProductResult = Vector3.Dot(car.transform.forward.normalized, _carRigidbody.velocity.normalized);
		if (_dotProductResult < -0.1)
		{
			ActivateBackLights();
		}
		else {
			DesactivateBackLights();
		}
	}

	public void ActivateBackLights() { 
		backLights.SetActive(true);
	}

	public void DesactivateBackLights() { 
		backLights.SetActive(false);
	}

}
