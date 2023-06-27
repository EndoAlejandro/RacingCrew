using System.Collections.Generic;
using Menu;
using UnityEngine;
using UnityEngine.Serialization;

public class LobbySelection : MonoBehaviour
{
    [FormerlySerializedAs("playerPrefab")] [SerializeField] private LobbyPlayer lobbyPlayerPrefab;

    private Dictionary<int, LobbyPlayer> _players = new();

    private void OnEnable()
    {
        PlayersManager.Instance.OnPlayerJoined += PlayersManagerOnPlayerJoined;
    }

    private void PlayersManagerOnPlayerJoined(PlayerInputSingle playerInput)
    {
        if (_players.ContainsKey(playerInput.Input.playerIndex)) return;

        var player = Instantiate(lobbyPlayerPrefab, transform);
        _players.Add(playerInput.Input.playerIndex, player);
    }
}