using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsernameConfirmation : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField username;
    [SerializeField]
    private Button button;
    public GameObject LobbyMenu;
    public GameObject UsernameMenu;

    void Start()
    {
        button.onClick.AddListener(UsernameConfirm);
    }

    private async void UsernameConfirm()
    {
        if (string.IsNullOrEmpty(username.text) || username.text.Length < 3)
        {
            Debug.LogWarning("Username cannot be empty and must be at least 3 characters long.");
            return;
        }
        PlayerPrefs.SetString("username", username.text);
        LobbyMenu.SetActive(true);
        UsernameMenu.SetActive(false);
    }
}