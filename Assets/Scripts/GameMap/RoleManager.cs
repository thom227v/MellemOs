using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;
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

    private void Start()
    {
        DisableKillAbility();
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
        Destroy(GameObject.Find(playerName));
        DisableKillAbility();
    }
}