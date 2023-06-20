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
            if (target == null) return;

            var horizontal = target.velocity;
            horizontal.y = 0f;
            _text.SetText(((int)horizontal.magnitude).ToString());
        }
    }
}