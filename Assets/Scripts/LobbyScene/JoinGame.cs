using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinGame : MonoBehaviour
{
    GameObject LobbyMenu;
    GameObject LobbyWaitingMenu;
    string LobbyNameText;
    string LobbyName;

    void Start()
    {
        gameObject.transform.Find("LobbyLabel").GetComponent<TMP_Text>().SetText(LobbyNameText);
        gameObject.GetComponentInChildren<Button>().onClick.AddListener(TaskOnClick);
    }

    public JoinGame Init(GameObject lobbyMenu, GameObject lobbyWaitingMenu, string text, string lobbyName)
    {
        LobbyMenu = lobbyMenu;
        LobbyWaitingMenu = lobbyWaitingMenu;
        LobbyNameText = text;
        LobbyName = lobbyName;
        return this;
    }

    private void TaskOnClick()
    {
        WSService.Instance.JoinLobby(LobbyName);
        LobbyMenu.SetActive(false);
        LobbyWaitingMenu.SetActive(true);
    }
}