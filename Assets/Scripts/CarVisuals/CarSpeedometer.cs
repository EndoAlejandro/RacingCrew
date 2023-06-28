using CarComponents;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Car))]
public class CarSpeedometer : MonoBehaviour
{
	[Header("SPEEDOMETER OBJECTS")]
	[SerializeField] private RectTransform arrow;
	[SerializeField] private TextMeshProUGUI textSpeed;
	[Header("SPEEPDOMETER PARAMETERS")]
	[SerializeField] private float minSpeedArrowAngle;
	[SerializeField] private float maxSpeedArrowAngle;

	private Rigidbody carRigidbody;
	private Car car;
	private float _maxSpeed;
	private float _speed = 0.0f;

	private void Awake()
	{
		carRigidbody = GetComponent<Rigidbody>();
		car = GetComponent<Car>();		
	}

	private void Start()
	{
		_maxSpeed = car.Stats.MaxSpeed;
		Debug.Log(_maxSpeed);
		SetSpeedometerText();
	}

	private void Update()
	{
		_speed = carRigidbody.velocity.magnitude;
		SetSpeedometerText();
		arrow.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle,maxSpeedArrowAngle, _speed/_maxSpeed));

	}

	private void SetSpeedometerText() {
		textSpeed.text = (int)_speed + " km/h";
	}




}
