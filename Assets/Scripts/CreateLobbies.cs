using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbies : MonoBehaviour
{
    public WSService WSService;
    public GameObject LobbyMenuPrefab;
    void Start()
    {
        WSService.OnLobbiesReceived += LobbiesReceived;
        LobbiesReceived(WSService.GetLobbies());
    }

    private void LobbiesReceived(Lobbies lobbies)
    {
        foreach (Lobby lobby in lobbies.Lobbier)
        {
            GameObject obj = Instantiate(LobbyMenuPrefab);
            obj.transform.SetParent(transform, false);
            obj.GetComponent<LobbyMenu>().Setup(lobby.Name);
            //obj.GetComponent<Button>().onClick.AddListener(() => WSService.JoinLobby(lobby));
        }
    }
}