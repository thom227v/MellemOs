using TMPro;
using UnityEngine;

public class LobbyWaitingLabel : MonoBehaviour
{
    async void OnEnable()
    {
        gameObject.GetComponent<TMP_Text>().text = await WSService.Instance.GetLobbyName();
    }
}