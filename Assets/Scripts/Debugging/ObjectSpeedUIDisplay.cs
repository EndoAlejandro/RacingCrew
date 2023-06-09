using System.Globalization;
using TMPro;
using UnityEngine;

namespace Debugging
{
    [RequireComponent(typeof(TMP_Text))]
    public class ObjectSpeedUIDisplay : MonoBehaviour
    {
        [SerializeField] private Rigidbody target;
        private TMP_Text _text;
        private void Awake() => _text = GetComponent<TMP_Text>();

        private void Update()
        {
            var horizontal = target.velocity;
            horizontal.y = 0f;
            _text.SetText(horizontal.magnitude.ToString(CultureInfo.InvariantCulture));
        }
    }
}