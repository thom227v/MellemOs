using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaveLobby : MonoBehaviour
{
    public GameObject LobbyMenu;
    public GameObject LobbyWaitingMenu;
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Leave);
    }

    private void Leave()
    {
        WSService.Instance.LeaveLobby();
        LobbyMenu.SetActive(true);
        LobbyWaitingMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
