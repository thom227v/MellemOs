using TMPro;
using UnityEngine;

public class LobbyWaiting : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField lobbyName;
    void Start()
    {
        WSService.Instance.JoinLobby(lobbyName.text);
    }
}