using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public TMP_Text lobbyName;
    void Start()
    {
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGameButton);
        WSService.Instance.OnGameStarted += OnGameStarted;
    }

    private void OnGameStarted(string lobbyName, int matchId)
    {
        Debug.Log("OnGameStarted event received for lobby: " + lobbyName + " with matchId: " + matchId);
        if (lobbyName == this.lobbyName.text)
        {
            Debug.Log("Game started for lobby: " + lobbyName);
            GameInformation2.matchId = matchId;
            LittleHelper.Instance.lobbyStarted = true;
                
        }

    }

    private async void StartGameButton()
    {
        int matchId = await WSService.Instance.StartGame(lobbyName.text);
        GameInformation2.matchId = matchId;
        SceneManager.LoadScene("GAMEMAP");
    }
}