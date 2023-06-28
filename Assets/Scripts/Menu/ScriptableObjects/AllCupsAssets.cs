using System.Collections;
using System.Collections.Generic;
using Menu.ScriptableObjects;
using UnityEngine;

namespace Menu {
	[CreateAssetMenu(menuName = "Scriptable Objects/ All Cups Assets", order = 3)]
	public class AllCupsAssets : ScriptableObject
	{
		public CupSelectionAssets[] cups;  
	}
}

