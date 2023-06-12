using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Menu {
	[CreateAssetMenu(menuName = "Scriptable Objects/Cup Selection Assets", order = 1)]
	public class CupSelectionAssets : ScriptableObject
	{
		public int cupID;
		public string cupName;
		public Sprite[] imageSpeedway = new Sprite[4];
	}
}

