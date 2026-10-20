using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewLobby : MonoBehaviour
{
    public TMP_InputField lobbyName;
    public TMP_InputField lobbySize;

    public GameObject LobbyWaitingMenu;
    public GameObject CreateLobbyMenu;
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(CreateLobby);
    }

    private async void CreateLobby()
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
        string username = PlayerPrefs.GetString("username");
        WSService.Instance.CreateLobby(lobbyName.text, size);
        WSService.Instance.JoinLobby(lobbyName.text);
        LobbyWaitingMenu.SetActive(true);
        CreateLobbyMenu.SetActive(false);
    }
}