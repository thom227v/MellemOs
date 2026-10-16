using System;
using UnityEngine;

public class OpenCreateLobbyMenu : MonoBehaviour
{
    public GameObject CreateLobbyMenu;
    void Start()
    {
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenMenu);
    }

    private void OpenMenu()
    {
        CreateLobbyMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
