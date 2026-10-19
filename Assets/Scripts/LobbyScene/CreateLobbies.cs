using System;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbies : MonoBehaviour
{
    public WSService WSService;
    public GameObject LobbyPrefab;
    public GameObject LobbyMenu;
    public GameObject LobbyWaitingMenu;
    public GameObject RefreshLobbiesButton;
    int timer = 0;

    void Start()
    {
        WSService.OnLobbiesReceived += LobbiesReceived;
        LobbiesReceived(WSService.GetLobbies());

        RefreshLobbiesButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            LobbiesReceived(WSService.GetLobbies());
        });
    }

    private void LobbiesReceived(Lobbies lobbies)
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbies.Lobbier)
        {
            GameObject obj = Instantiate(LobbyPrefab);
            string text = lobby.Name + " (" + lobby.Players.Count + "/" + lobby.MaxPlayers + ")";
            obj.GetComponentInChildren<JoinGame>().Init(LobbyMenu, LobbyWaitingMenu, text, lobby.Name);
            obj.transform.SetParent(transform, false);
        }
    }
}