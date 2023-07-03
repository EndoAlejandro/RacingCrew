using UnityEngine;

namespace Menu.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/ All Cups Assets", order = 3)]
    public class AllCupsAssets : ScriptableObject
    {
        [SerializeField] private CupSelectionAssets[] cups;
        public CupSelectionAssets[] Cups => cups;
    }
}