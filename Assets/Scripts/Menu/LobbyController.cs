using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Menu
{
    public class LobbyController : MonoBehaviour
    {
        [SerializeField] private LobbyPlayer lobbyPlayerPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private GameObject startRaceButton;

        private List<LobbyPlayer> _lobbyPlayers;

        private bool _allPlayersReady;

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

        private void LobbyPlayerOnLobbyPlayerReady()
        {
            var wasAllReady = _allPlayersReady;
            _allPlayersReady = true;
            foreach (var _ in _lobbyPlayers.Where(lobbyPlayer => !lobbyPlayer.IsPlayerReady))
                _allPlayersReady = false;

            if (wasAllReady && _allPlayersReady) GameManager.Instance.StartCup();
            
            startRaceButton.gameObject.SetActive(_allPlayersReady);
        }

        private void PlayersManagerOnPlayerJoined(PlayerInputSingle playerInput)
        {
            if (playerInput.PlayerIndex < 0)
                return;
            if (_lobbyPlayers.Any(lobbyPlayer =>
                    lobbyPlayer.PlayerInputSingle.Input.playerIndex == playerInput.Input.playerIndex))
                return;

            AddLobbyPlayer(playerInput);
        }

        private void PlayersManagerOnPlayerDisconnected(PlayerInputSingle playerInput)
        {
            if (playerInput.PlayerIndex < 0) return;
            foreach (var lobbyPlayer in _lobbyPlayers)
            {
                if (lobbyPlayer.PlayerInputSingle.PlayerIndex != playerInput.PlayerIndex) continue;

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
    }
}