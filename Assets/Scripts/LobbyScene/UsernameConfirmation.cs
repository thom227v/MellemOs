using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsernameConfirmation : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField username;
    public GameObject LobbyMenu;
    public GameObject UsernameMenu;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(UsernameConfirm);
    }

    private async void UsernameConfirm()
    {
        if (string.IsNullOrEmpty(username.text) || username.text.Length < 3)
        {
            Debug.LogWarning("Username cannot be empty and must be at least 3 characters long.");
            return;
        }
        bool isUsernameTaken = await WSService.Instance.UsernameCheck(username.text);
        if (isUsernameTaken)
        {
            Debug.LogWarning("Username is already taken.");
            return;
        }
        Debug.Log("Username confirmed: " + username.text);
        GameInformation2.username = username.text;
        LobbyMenu.SetActive(true);
        UsernameMenu.SetActive(false);
    }
}