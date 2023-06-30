using UnityEngine;

namespace Menu.ScriptableObjects {
	[CreateAssetMenu(menuName = "Scriptable Objects/ Car Assets", order = 2)]
	public class CarAssets : ScriptableObject
	{
		public GameObject[] cars; 
	}

}
