using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayersInLobbyWaiting : MonoBehaviour
{

    async void OnEnable()
    {
        await LobbyManager.Instance.UpdatePlayers();
    }

}