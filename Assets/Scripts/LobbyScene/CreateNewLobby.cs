using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewLobby : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(LobbyManager.Instance.CreateLobby);
    }
}