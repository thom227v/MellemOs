using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RoleManager : MonoBehaviour
{
    public GameObject killButton;
    public static RoleManager Instance { get; private set; }
    private string targetPlayerName;
    public GameObject DeadPlayerPrefab;

    private void Awake()
    {
        Instance = this;
        killButton.GetComponent<Button>().onClick.AddListener(KillCurrentPlayer);
    }

    public void DisableKillAbility()
    {
        targetPlayerName = null;
        killButton.SetActive(false);
    }

    public void EnableKillAbility(string playerName)
    {
        targetPlayerName = playerName;
        killButton.SetActive(true);
    }

    private async void KillCurrentPlayer()
    {
        if (!string.IsNullOrEmpty(targetPlayerName))
            await KillPlayer(targetPlayerName);
    }

    private async Task KillPlayer(string playerName)
    {
        await WSGameMap.Instance.KillPlayer(playerName);

        GameObject playerObject = GameObject.Find(playerName);
        GameObject deadPlayer = Instantiate(DeadPlayerPrefab, playerObject.transform.position, Quaternion.identity);
        deadPlayer.name = playerName;
    }
}