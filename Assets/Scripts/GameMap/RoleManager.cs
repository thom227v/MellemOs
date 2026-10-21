using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RoleManager : MonoBehaviour
{
    public GameObject killButton;
    public static RoleManager Instance { get; private set; }

    public RoleManager()
    {
        Instance = this;
    }

    public void DisableKillAbility()
    {
        killButton.SetActive(false);
    }

    public void EnableKillAbility(string playerName)
    {
        killButton.SetActive(true);
        killButton.GetComponent<Button>().onClick.AddListener(async () => await KillPlayer(playerName));
    }

    private async Task KillPlayer(string playerName)
    {
        await WSService.Instance.KillPlayer(playerName);
    }
}