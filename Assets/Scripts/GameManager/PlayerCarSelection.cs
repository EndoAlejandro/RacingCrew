using Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame {
	public class PlayerCarSelection : MonoBehaviour
	{
		public static PlayerCarSelection Instance;
		public static int playerID = 0;
		[SerializeField] CarAssets carAssets;

		private int _id;


		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			playerID++;
			_id = playerID;
			GameObject car = Instantiate(CarSelected(_id));
			car.transform.SetParent(gameObject.transform);
		}

		public GameObject CarSelected(int ID) {
			return carAssets.cars[PlayerPrefs.GetInt("Player" + ID.ToString())];
		}
	}
}

