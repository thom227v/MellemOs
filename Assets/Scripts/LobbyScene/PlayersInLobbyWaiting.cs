using System;
using System.Linq;
using UnityEngine;

public class PlayersInLobbyWaiting : MonoBehaviour
{
    private bool PlayersUpdated = false;
    string[] playersInLobby = null;
    void Start()
    {
        if (WSService.Instance != null)
        {
            WSService.Instance.LobbyPlayersRecieved += UpdateLobbyPlayers;
        }
        else
        {
            Debug.LogError("WSService.Instance is null. Cannot subscribe to LobbyPlayersRecieved.");
        }
    }

    void Update()
    {
        if (PlayersUpdated)
        {
            LobbyManager.Instance.CreatePlayers(playersInLobby);
            PlayersUpdated = false;
        }
    }

    async void OnEnable()
    {
        await LobbyManager.Instance.UpdatePlayers();
    }

    private void UpdateLobbyPlayers(string[] players)
    {
        string username = GameInformation2.username;
        if (players.Contains(username))
        {
            playersInLobby = players;
            PlayersUpdated = true;
        }
    }
}