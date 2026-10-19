using System;
using TMPro;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private TMP_Text lobbyName;
    void Start()
    {
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGameButton);
    }

    private void StartGameButton()
    {
        WSService.Instance.StartGame(lobbyName.text);
    }
}