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
    }

    private void StartGameButton()
    {
        WSService.Instance.StartGame(lobbyName.text);
        SceneManager.LoadScene("GAMEMAP");
    }
}