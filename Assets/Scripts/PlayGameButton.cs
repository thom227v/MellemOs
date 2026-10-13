using UnityEngine;
using UnityEngine.UI;

public class PlayGameButton : MonoBehaviour
{
    public GameObject menuUI;
    public Button playButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        // Mock for now, should have opened websocket normally.
        menuUI.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {

    }
}
