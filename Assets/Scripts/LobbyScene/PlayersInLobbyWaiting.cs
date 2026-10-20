using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayersInLobbyWaiting : MonoBehaviour
{
    public GameObject PlayerName;

    public static PlayersInLobbyWaiting Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }


    public async Task UpdatePlayers()
    {
        string[] playersNames = await WSService.Instance.GetPlayersInCurrentLobby();
        CreatePlayers(playersNames);
    }

    void CreatePlayers(string[] playersNames)
    {
        if (gameObject.transform.childCount > 0)
        { 
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (string playerName in playersNames)
        {
            Debug.Log("Creating player: " + playerName);
            GameObject playerObj = Instantiate(PlayerName);
            playerObj.GetComponent<TMPro.TMP_Text>().text = playerName;
            playerObj.transform.SetParent(this.transform);
        }
    }
}