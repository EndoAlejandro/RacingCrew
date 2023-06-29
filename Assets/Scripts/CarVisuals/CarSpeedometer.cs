using RaceComponents;
using TMPro;
using UnityEngine;

namespace CarVisuals
{
    [RequireComponent(typeof(PlayerViewController))]
    public class CarSpeedometer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI positionText;

        [SerializeField] private RectTransform arrow;
        [SerializeField] private TextMeshProUGUI textSpeed;

        [SerializeField] private float minSpeedArrowAngle;
        [SerializeField] private float maxSpeedArrowAngle;

        private float _maxSpeed;
        private float _speed = 0.0f;

        private PlayerViewController _playerViewController;
        private Rigidbody _carRigidbody;

        private void Awake() => _playerViewController = GetComponent<PlayerViewController>();

        private void Start()
        {
            _carRigidbody = _playerViewController.Car.GetComponent<Rigidbody>();
            _maxSpeed = _playerViewController.Car.Stats.MaxSpeed;
            SetSpeedometerText();
        }

        private void Update()
        {
            SetPosition();
            _speed = _carRigidbody.velocity.magnitude;
            SetSpeedometerText();
            arrow.localEulerAngles =
                new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, _speed / _maxSpeed));
        }

        private void SetPosition()
        {
            var racersCount = TrackManager.Instance.RacersPositions.Count;
            var position = TrackManager.Instance.GetPosition(_playerViewController.Car.RacerPosition);
            positionText.SetText(position + "/" + racersCount);
        }

        private void SetSpeedometerText() => textSpeed.text = (int)_speed + " km/h";
    }
}