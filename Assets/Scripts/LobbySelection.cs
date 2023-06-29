using System.Collections.Generic;
using Menu;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbySelection : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;

    private Dictionary<int, Player> _players = new();

    private void OnEnable()
    {
        PlayersManager.Instance.OnPlayerJoined += PlayersManagerOnPlayerJoined;
    }

    private void PlayersManagerOnPlayerJoined(PlayerInput playerInput)
    {
        if (!_players.ContainsKey(playerInput.playerIndex))
        {
            var player = Instantiate(playerPrefab, transform);
            _players.Add(playerInput.playerIndex, player);
        }

        /*if (_players[playerInput.playerIndex] == null)
            _players[playerInput.playerIndex] = Instantiate(playerPrefab, transform);*/
    }
}