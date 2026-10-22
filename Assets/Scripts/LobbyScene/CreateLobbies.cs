using UnityEngine;
public class CreateLobbies : MonoBehaviour
{
    public WSService WSService;
    private Lobbies LobbiesNotStarted;
    private bool LobiesUpdated = false;

    async void Start()
    {
        if (WSService.Instance != null)
        {
            WSService.Instance.OnLobbiesReceived += UpdateLobbies;
        }
        else
        {
            Debug.LogError("WSGameMap.Instance is null. Cannot subscribe to OnLobbiesReceived.");
        }
    }

    void Update()
    {
        if (LobiesUpdated)
        {
            LobiesUpdated = false;
            LobbyManager.Instance.LobbiesReceived(LobbiesNotStarted);
        }
    }

    private async void GetLobbies()
    {
        Lobbies lobbies = await WSService.GetLobbiesNotStarted();
        LobbyManager.Instance.LobbiesReceived(lobbies);
    }

    void OnEnable()
    {
        GetLobbies();
    }

    private void UpdateLobbies(Lobbies lobbies)
    {
        LobbiesNotStarted = lobbies;
        LobiesUpdated = true;
    }
}