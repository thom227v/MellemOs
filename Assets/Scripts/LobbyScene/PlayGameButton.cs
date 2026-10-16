using UnityEngine;
using UnityEngine.UI;

public class PlayGameButton : MonoBehaviour
{
    public GameObject menuUI;
    public Button playButton;
    public GameObject LobbyMenu = GameObject.Find("LobbyMenu");
    public GameObject PlayGame = GameObject.Find("PlayGame");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        // Mock for now, should have opened websocket normally.
        LobbyMenu.SetActive(true);
        PlayGame.SetActive(false);
        //menuUI.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {

    }
}
