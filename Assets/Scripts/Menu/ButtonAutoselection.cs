using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu{
	public class ButtonAutoselection : MonoBehaviour
	{
		[SerializeField] private Button button;

		private void Awake()
		{
			button = GetComponent<Button>();
		}

		private void OnEnable()
		{

			button.Select();
		}
	}
}

