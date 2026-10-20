using TMPro;
using UnityEngine;

public class LobbyWaitingLabel : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<TMP_Text>().text = WSService.Instance.GetLobbyName();
    }
}
