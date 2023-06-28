using UnityEngine;

namespace CarComponents
{
    [CreateAssetMenu(menuName = "Scriptable Objects/New Car Data", fileName = "NewCarData", order = 3)]
    public class CarData : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField] private CarStats stats;

        [Header("Visuals")]
        [SerializeField] private GameObject[] models;

        public CarStats Stats => stats;
        public GameObject[] Models => models;
    }
}