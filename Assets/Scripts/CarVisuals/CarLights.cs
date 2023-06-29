using UnityEngine;

namespace CarVisuals
{
    public class CarLights : MonoBehaviour
    {
        [SerializeField] private GameObject backLights;
        private Rigidbody _rigidbody;
        private float _dotProduct;
        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        private void Update()
        {
            _dotProduct = Vector3.Dot(_rigidbody.transform.forward.normalized, _rigidbody.velocity.normalized);
            backLights.SetActive(_dotProduct < -0.1);
        }
    }
}