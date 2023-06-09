using System.Collections.Generic;
using System.Linq;
using InputManagement;
using UnityEngine;

namespace Menu
{
    public class LobbyController : MonoBehaviour
    {
        [SerializeField] private LobbyPlayer lobbyPlayerPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private GameObject startRaceButton;

        private List<LobbyPlayer> _lobbyPlayers;

        private bool _allReady;

        private void OnEnable()
        {
            _lobbyPlayers = new List<LobbyPlayer>();
            PlayersManager.Instance.SetState(PlayersManager.State.Lobby);

            foreach (var playerInputSingle in PlayersManager.Instance.PlayerInputs)
                AddLobbyPlayer(playerInputSingle);

            PlayersManager.Instance.OnPlayerJoined += PlayersManagerOnPlayerJoined;
            PlayersManager.Instance.OnPlayerDisconnected += PlayersManagerOnPlayerDisconnected;

            LobbyPlayer.OnLobbyPlayerReady += LobbyPlayerOnLobbyPlayerReady;
        }

        private void LobbyPlayerOnLobbyPlayerReady(bool wasLockAction)
        {
            if (_allReady && wasLockAction)
                GameManager.Instance.StartCup();

            CheckForAllReady();
            startRaceButton.gameObject.SetActive(_allReady);
        }

        private void CheckForAllReady()
        {
            _allReady = true;
            foreach (var _ in _lobbyPlayers.Where(lobbyPlayer => !lobbyPlayer.IsPlayerReady))
                _allReady = false;
        }

        private void PlayersManagerOnPlayerJoined(PlayerInputSingle playerInput)
        {
            if (playerInput.PlayerIndex < 0)
                return;
            if (_lobbyPlayers.Any(lobbyPlayer =>
                    lobbyPlayer.PlayerInputSingle.Input.playerIndex == playerInput.Input.playerIndex))
                return;

            AddLobbyPlayer(playerInput);
            CheckForAllReady();
            startRaceButton.gameObject.SetActive(_allReady);
        }

        private void PlayersManagerOnPlayerDisconnected(PlayerInputSingle playerInput)
        {
            if (playerInput.PlayerIndex < 0)
                return;
            foreach (var lobbyPlayer in _lobbyPlayers)
            {
                if (lobbyPlayer.PlayerInputSingle.PlayerIndex != playerInput.PlayerIndex)
                    continue;

                RemoveLobbyPlayer(lobbyPlayer);
                break;
            }
        }

        private void AddLobbyPlayer(PlayerInputSingle inputSingle)
        {
            var lobbyPlayer = Instantiate(lobbyPlayerPrefab, container);
            lobbyPlayer.Setup(inputSingle);
            _lobbyPlayers.Add(lobbyPlayer);
        }

        private void RemoveLobbyPlayer(LobbyPlayer lobbyPlayer)
        {
            _lobbyPlayers.Remove(lobbyPlayer);
            Destroy(lobbyPlayer.gameObject);
        }

        private void OnDestroy()
        {
            PlayersManager.Instance.OnPlayerJoined -= PlayersManagerOnPlayerJoined;
            PlayersManager.Instance.OnPlayerDisconnected -= PlayersManagerOnPlayerDisconnected;

            LobbyPlayer.OnLobbyPlayerReady -= LobbyPlayerOnLobbyPlayerReady;
        }
    }
}