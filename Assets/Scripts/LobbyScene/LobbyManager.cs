using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;
    public CreateLobbies CreateLobbies;
    public PlayersInLobbyWaiting PlayersInLobbyWaiting;
    public GameObject CreateLobbyMenu;
    public GameObject LobbyPrefab;
    public GameObject LobbyMenu;
    public GameObject LobbyWaitingMenu;
    public GameObject PlayerNamePrefab;
    public TMP_InputField lobbyName;
    public TMP_InputField lobbySize;

    void Awake()
    {
        Instance = this;
    }

    public async Task<string[]> GetPlayersInCurrentLobby()
    {
        string[] playerNames = await WSService.Instance.GetPlayersInCurrentLobby();
        return playerNames;
    }

    public void LobbiesReceived(Lobbies lobbies)
    {
        foreach (Transform child in CreateLobbies.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbies.Lobbier)
        {
            GameObject obj = Instantiate(LobbyPrefab);
            string text = lobby.Name + " (" + lobby.Players.Count + "/" + lobby.MaxPlayers + ")";
            obj.GetComponentInChildren<JoinGame>().Init(LobbyMenu, LobbyWaitingMenu, text, lobby.Name);
            obj.transform.SetParent(CreateLobbies.transform, false);
        }
    }

    public async Task UpdatePlayers()
    {
        string[] playersNames = await WSService.Instance.GetPlayersInCurrentLobby();
        CreatePlayers(playersNames);
    }

    public void CreatePlayers(string[] playersNames)
    {
        if (PlayersInLobbyWaiting.transform.childCount > 0)
        {
            foreach (Transform child in PlayersInLobbyWaiting.transform)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (string playerName in playersNames)
        {
            GameObject playerObj = Instantiate(PlayerNamePrefab);
            playerObj.GetComponent<TMP_Text>().text = playerName;
            playerObj.transform.SetParent(PlayersInLobbyWaiting.transform, false);
        }
    }

    public async void CreateLobby()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }


        if (string.IsNullOrEmpty(lobbyName.text) || lobbyName.text.Length < 3)
        {
            Debug.LogWarning("Lobby name cannot be empty and must be at least 3 characters long.");
            return;
        }
        if (string.IsNullOrEmpty(lobbySize.text) || !int.TryParse(lobbySize.text, out int size) || size < 2)
        {
            Debug.LogWarning("Lobby size must be a number and at least 2.");
            return;
        }
        string username = GameInformation2.username;
        await WSService.Instance.CreateLobby(lobbyName.text, size);
        await WSService.Instance.JoinLobby(lobbyName.text);
        LobbyWaitingMenu.SetActive(true);
        CreateLobbyMenu.SetActive(false);
    }

    public void TaskOnClick(string lobbyName)
    {
        JoinLobby(lobbyName);
        LobbyMenu.SetActive(false);
        LobbyWaitingMenu.SetActive(true);
    }

    public async void JoinLobby(string lobbyName)
    {
        await WSService.Instance.JoinLobby(lobbyName);
    }
}