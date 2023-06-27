using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class ButtonAutoSelection : MonoBehaviour
    {
        private Selectable _selectable;
        private void Awake() => _selectable = GetComponent<Selectable>();
        private void OnEnable() => _selectable.Select();
    }
}