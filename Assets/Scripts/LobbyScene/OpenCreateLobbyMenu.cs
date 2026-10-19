using System;
using UnityEngine;

public class OpenCreateLobbyMenu : MonoBehaviour
{
    public GameObject CreateLobbyMenu;
    public GameObject LobbyMenu;
    void Start()
    {
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenMenu);
    }

    private void OpenMenu()
    {
        CreateLobbyMenu.SetActive(true);
        LobbyMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
