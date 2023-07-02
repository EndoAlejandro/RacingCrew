using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    public class SelectableAutoSelect : MonoBehaviour
    {
        private Selectable _selectable;
        private void Awake() => _selectable = GetComponent<Selectable>();
        private void OnEnable() => _selectable.Select();

        private void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null) _selectable.Select();
        }
    }
}