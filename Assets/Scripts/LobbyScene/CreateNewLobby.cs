using System;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewLobby : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField lobbyName;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(CreateLobby);
    }

    private void CreateLobby()
    {
        if (lobbyName == null || string.IsNullOrEmpty(lobbyName.text) || lobbyName.text.Length < 3)
        {
            Debug.LogWarning("Lobby name cannot be empty and must be at least 3 characters long.");
            return;
        }
        string username = PlayerPrefs.GetString("username");
        WSService.Instance.CreateLobby(lobbyName.text);
    }

    void Update()
    {
        
    }
}
