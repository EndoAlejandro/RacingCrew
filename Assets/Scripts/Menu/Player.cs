using CarComponents;
using InGame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Menu
{
    public class Player : MonoBehaviour
    {
        public static int playerID = 0;

        [SerializeField] private TextMeshProUGUI playerIDText;
        [SerializeField] private Transform carTransform;
        [SerializeField] private CarData carData;
        [SerializeField] private Button[] buttons;


        private int _id;
        private List<GameObject> _carList = new List<GameObject>();
        private int _index = 0;
        private bool _selection = true;
        private bool _readyToSelection = false;

        private Transform _parent;

        private PlayerInput _input;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _parent = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
            gameObject.transform.SetParent(_parent, false);
        }

        private void Start()
        {
            playerID++;
            _id = playerID;
            playerIDText.text = "PLAYER " + _id.ToString();

            for (int i = 0; i < carData.Models.Length; i++)
            {
                GameObject car = Instantiate(carData.Models[i], new Vector3(0, 99, -60),
                    Quaternion.Euler(23, 122, -32));
                car.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                car.transform.parent = carTransform;
                car.transform.localPosition = Vector3.zero;
                car.SetActive(false);
                if (i == 0)
                {
                    car.SetActive(true);
                }

                _carList.Add(car);
            }
        }

        #region INPUT SYSTEM

        public void OnNextOption()
        {
            if (_selection)
            {
                NextCar();
            }
        }

        public void OnPrevOption()
        {
            if (_selection)
            {
                PrevCar();
            }
        }

        public void NextCar()
        {
            _carList[_index].SetActive(false);
            _index++;
            if (_index >= _carList.Count)
            {
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
                _index = _carList.Count - 1;
            }

            _carList[_index].SetActive(true);
        }

        public void OnSelect()
        {
            if ( /*playerID == 1 &&*/ _readyToSelection)
            {
                //Guarda el index del coche que el jugador selecciono
                GameManager.Instance.AddPlayerCarModel(_input, carData.Models[_index]);

                _selection = false;

                for (int i = 0; i < buttons.Length; ++i)
                {
                    buttons[i].interactable = false;
                }

                MenuManager.Instance.PlayersReady++;
            }
            else
            {
                _readyToSelection = true;
            }
        }

        #endregion
    }
}