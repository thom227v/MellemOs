using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewLobby : MonoBehaviour
{

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(CreateLobby);
    }

    private void CreateLobby()
    {
        string username = PlayerPrefs.GetString("username", "Player");
        WSService.Instance.CreateLobby("NewLobby");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
