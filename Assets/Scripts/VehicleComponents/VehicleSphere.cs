using UnityEngine;

namespace VehicleComponents
{
    public class VehicleSphere : MonoBehaviour
    {
        [SerializeField] private Transform display;
        [SerializeField] private float minDriftSpeed = 20f;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float turnFactor = 20f;
        [SerializeField] private float acceleration = 1f;

        private float _currentSpeed;
        private float _realSpeed;
        private float _steerDirection;
        private float _driftTime;

        private bool _driftLeft;
        private bool _driftRight;
        private bool _isSliding;
        private bool _grounded;

        private const float OutwardsDriftForce = 50000f;

        private Rigidbody _rigidbody;

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        private void Update()
        {
            GroundNormalRotation();
        }

        private void FixedUpdate()
        {
            // if (!_touchingGround) return;
            Move();
            Steer();
            Drift();
        }

        private void Drift()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && _grounded)
            {
                _driftRight = _steerDirection > 0f;
                _driftLeft = _steerDirection < 0f;
                _isSliding = true;
            }

            if (Input.GetKey(KeyCode.LeftShift) && _grounded && _currentSpeed > minDriftSpeed &&
                Input.GetAxis("Horizontal") != 0)
            {
                _driftRight = _steerDirection > 0f;
                _driftLeft = _steerDirection < 0f;
                _isSliding = true;
                _driftTime += Time.deltaTime;
            }

            if (!Input.GetKey(KeyCode.LeftShift) || _realSpeed < minDriftSpeed)
            {
                _driftLeft = false;
                _driftRight = false;
                _isSliding = false;

                _driftTime = 0f;
            }
        }

        private void GroundNormalRotation()
        {
            var hit = new RaycastHit();
            if (Physics.Raycast(display.position, -transform.up, out hit, 1f))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.FromToRotation(transform.up * 2, hit.normal) * transform.rotation,
                    7.5f * Time.deltaTime);
                _grounded = true;
            }
            else
            {
                _grounded = false;
            }
        }

        private void Steer()
        {
            _steerDirection = Input.GetAxisRaw("Horizontal");

            if (_driftLeft && !_driftRight)
            {
                _steerDirection = Input.GetAxis("Horizontal") < 0 ? -1.5f : -0.5f;
                var targetRotation = Quaternion.Euler(0f, -turnFactor, 0f);
                display.localRotation = Quaternion.Lerp(display.localRotation, targetRotation, Time.deltaTime * 8f);

                if (_isSliding && _grounded)
                    _rigidbody.AddForce(transform.right * OutwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);
            }
            else if (_driftRight && !_driftLeft)
            {
                _steerDirection = Input.GetAxis("Horizontal") > 0f ? 1.5f : 0.5f;
                var targetRotation = Quaternion.Euler(0, turnFactor, 0);
                display.localRotation = Quaternion.Lerp(display.localRotation, targetRotation, Time.deltaTime * 8f);

                if (_isSliding && _grounded)
                    _rigidbody.AddForce(transform.right * -OutwardsDriftForce * Time.deltaTime,
                        ForceMode.Acceleration);
            }
            else
            {
                var targetRotation = Quaternion.Euler(Vector3.zero);
                display.localRotation = Quaternion.Lerp(display.localRotation, targetRotation, Time.deltaTime * 8f);
            }

            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation,
                Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0), 2 * Time.deltaTime);
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation,
                Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z), 2 * Time.deltaTime);

            var steerAmount =
                _realSpeed > 30f ? _realSpeed / 4f * _steerDirection : _realSpeed / 1.5f * _steerDirection;
            var steerDirectionVector = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + steerAmount,
                transform.eulerAngles.z);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, steerDirectionVector, Time.deltaTime * 3f);
        }

        private void Move()
        {
            _realSpeed = transform.InverseTransformDirection(_rigidbody.velocity).z;

            if (Input.GetKey(KeyCode.W))
                _currentSpeed = Mathf.Lerp(_currentSpeed, maxSpeed, Time.deltaTime * acceleration);
            else if (Input.GetKey(KeyCode.S))
                _currentSpeed = Mathf.Lerp(_currentSpeed, -maxSpeed / 1.75f, Time.deltaTime);
            else
                _currentSpeed = Mathf.Lerp(_currentSpeed, 0f, Time.deltaTime * 1.5f);

            var velocity = display.forward * _currentSpeed;
            velocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = velocity;
        }
    }
}