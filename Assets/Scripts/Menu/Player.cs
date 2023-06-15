using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Menu {
	public class Player : MonoBehaviour
	{
		public static int playerID = 0;
		public TextMeshProUGUI _playerIDText;
		public Transform _carTransform;
		public CarAssets carAssets;


		private int _ID;
		private List<GameObject> _carList = new List<GameObject>();
		private int _index = 0;

		private Transform _parent;

		private void Awake()
		{
			_parent = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
			gameObject.transform.SetParent(_parent, false);
		}
		private void Start()
		{
			playerID++;
			_ID = playerID;
			_playerIDText.text = "PLAYER " + _ID.ToString();


			for (int i = 0; i < carAssets.cars.Length; i++) {
				GameObject car = Instantiate(carAssets.cars[i]);
				car.transform.parent = _carTransform;
				car.transform.localPosition = Vector3.zero;
				car.SetActive(false);
				if (i == 0) {
					car.SetActive(true);
				}
				_carList.Add(car);

			}

		}

		#region INPUT SYSTEM
		public void OnNextOption() {
			NextCar();
		}

		public void OnPrevOption() {
			PrevCar();
		}

		public void NextCar()
		{
			_carList[_index].SetActive(false);
			_index++;
			if (_index >= _carList.Count) { 
				_index = 0;
			}
			_carList[_index].SetActive(true);
		}

		public void PrevCar()
		{
			_carList[_index].SetActive(false);
			_index--;
			if (_index < 0)
			{
				_index = _carList.Count-1;
			}
			_carList[_index].SetActive(true);

		}
		#endregion

	}
}

