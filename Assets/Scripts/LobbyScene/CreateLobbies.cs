using UnityEngine;
using UnityEngine.UI;
public class CreateLobbies : MonoBehaviour
{
    public WSService WSService;
    public GameObject RefreshLobbiesButton;

    async void Start()
    {
        WSService.OnLobbiesReceived += LobbyManager.Instance.LobbiesReceived;
        GetLobbies();

        RefreshLobbiesButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            GetLobbies();
        });
    }

    private async void GetLobbies()
    {
        Lobbies lobbies = await WSService.GetLobbiesNotStarted();
        LobbyManager.Instance.LobbiesReceived(lobbies);
    }
}