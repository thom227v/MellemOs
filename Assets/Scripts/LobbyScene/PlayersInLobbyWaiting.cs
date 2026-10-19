using System;
using UnityEngine;

public class PlayersInLobbyWaiting : MonoBehaviour
{
    public GameObject PlayerName;

    async void Start()
    {
        string[] playersNames = await WSService.Instance.GetPlayersInCurrentLobby();
        CreatePlayers(playersNames);
    }

    void CreatePlayers(string[] playersNames)
    {
        foreach (string playerName in playersNames)
        {
            Debug.Log("Creating player: " + playerName);
            GameObject playerObj = Instantiate(PlayerName);
            playerObj.transform.SetParent(this.transform);
        }
    }
}