using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject LobbyMenu;
    public GameObject PlayGame = GameObject.Find("PlayGameButton");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponentInChildren<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        GameObject PlayGameButton = gameObject.GetComponentInChildren<Button>().gameObject;
        PlayGameButton.SetActive(false);
        LobbyMenu.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
