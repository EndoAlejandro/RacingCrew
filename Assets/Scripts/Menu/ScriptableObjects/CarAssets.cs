using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu {
	[CreateAssetMenu(menuName = "Scriptable Objects/ Car Assets", order = 2)]
	public class CarAssets : ScriptableObject
	{
		public GameObject[] cars; 
	}

}
