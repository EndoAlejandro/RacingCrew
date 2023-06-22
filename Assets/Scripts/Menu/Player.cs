using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu {
	public class Player : MonoBehaviour
	{
		public static int playerID = 0;

		[SerializeField] TextMeshProUGUI playerIDText;
		[SerializeField] Transform carTransform;
		[SerializeField] CarAssets carAssets;
		[SerializeField] Button[] buttons;


		private int _id;
		private List<GameObject> _carList = new List<GameObject>();
		private int _index = 0;
		private bool _selection = true;

		private Transform _parent;

		private void Awake()
		{
			_parent = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
			gameObject.transform.SetParent(_parent, false);
		}
		private void Start()
		{
			playerID++;
			_id = playerID;
			playerIDText.text = "PLAYER " + _id.ToString();


			for (int i = 0; i < carAssets.cars.Length; i++) {
				GameObject car = Instantiate(carAssets.cars[i]);
				car.transform.parent = carTransform;
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
			if (_selection) {
				NextCar();
			}
			
		}

		public void OnPrevOption() {
			if (_selection) {
				PrevCar();
			}		
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

		public void OnSelect()
		{

			//Guarda la información del jugador y el index del carro que eligió
			PlayerPrefs.SetInt("Player" + _id.ToString(),_index);

			_selection = false;

			for (int i = 0; i < buttons.Length;++i) {
				buttons[i].interactable = false;
			}

			MenuManager.Instance.PlayersReady++;

		}
		#endregion

	}
}

