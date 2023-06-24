using Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUtils;

namespace InGame {
	public class PlayerCarSelection : Singleton<PlayerCarSelection>
	{
		public static int playerID = 0;
		[SerializeField] private CarAssets carAssets;

		private int _id;

		protected override void Awake()
		{
			base.Awake();
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

