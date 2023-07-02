using System;
using CarComponents;
using System.Collections.Generic;
using InputManagement;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class LobbyPlayer : MonoBehaviour
    {
        public static event Action<bool> OnLobbyPlayerReady;

        [SerializeField] private TextMeshProUGUI playerDisplayText;
        [SerializeField] private GameObject readyText;
        [SerializeField] private Transform modelsContainer;
        [SerializeField] private CarData carData;

        private readonly List<GameObject> _models = new();
        private int _modelIndex;

        public PlayerInputSingle PlayerInputSingle { get; private set; }
        public bool IsPlayerReady { get; private set; }

        private void CreateModels()
        {
            var modelRotation = Quaternion.Euler(23, 122, -32);

            for (int i = 0; i < carData.Models.Length; i++)
            {
                var carModel = Instantiate(carData.Models[i], Vector3.zero, modelRotation);
                carModel.transform.localScale = Vector3.one * 0.45f;
                carModel.transform.SetParent(modelsContainer);
                carModel.transform.localPosition = Vector3.zero;
                carModel.SetActive(i == _modelIndex);

                _models.Add(carModel);
            }
        }

        public void Setup(PlayerInputSingle playerInputSingle)
        {
            PlayerInputSingle = playerInputSingle;
            playerDisplayText.text = "Player " + playerInputSingle.PlayerIndex;
            CreateModels();

            PlayerInputSingle.OnInputTriggered += PlayerInputSingleOnInputTriggered;
        }

        private void PlayerInputSingleOnInputTriggered()
        {
            if (PlayerInputSingle.MainMenuInputReader.Select)
                SetLockSelection(true);
            else if (PlayerInputSingle.MainMenuInputReader.Back)
                SetLockSelection(false);

            if (IsPlayerReady) return;
            ChangeSelectedCar(PlayerInputSingle.MainMenuInputReader.Navigation);
        }

        private void SetLockSelection(bool lockSelection)
        {
            if (lockSelection)
            {
                PlayerInputSingle.SetModelIndex(_models[_modelIndex]);
                PlayerInputSingle.SetCarData(carData.Stats);
            }

            IsPlayerReady = lockSelection;
            OnLobbyPlayerReady?.Invoke(lockSelection);
            readyText.SetActive(IsPlayerReady);
        }

        private void ChangeSelectedCar(int direction)
        {
            _models[_modelIndex].SetActive(false);
            _modelIndex = (_modelIndex + direction) % _models.Count;
            if (_modelIndex < 0) _modelIndex = _models.Count - 1;
            _models[_modelIndex].SetActive(true);
        }

        private void OnDestroy() => PlayerInputSingle.OnInputTriggered -= PlayerInputSingleOnInputTriggered;
    }
}