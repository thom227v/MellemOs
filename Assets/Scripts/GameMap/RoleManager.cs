using System;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoleManager : MonoBehaviour
{
    public GameObject killButton;
    public TMP_Text killButtonTxt;
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

    public DateTime canKillAgain = DateTime.MinValue;
    public void EnableKillAbility(string playerName)
    {
        if (DateTime.Now > canKillAgain)
        {
            targetPlayerName = playerName;
            killButtonTxt.text = "KILL";
        }
        else
        {
            killButtonTxt.text = "KILL ON COOLDOWN";
        }
        

        killButton.SetActive(true);
    }

    private async void KillCurrentPlayer()
    {
        if (!string.IsNullOrEmpty(targetPlayerName))
            await KillPlayer(targetPlayerName);
    }

    private async Task KillPlayer(string playerName)
    {
        canKillAgain = DateTime.Now.AddSeconds(10);
        await WSGameMap.Instance.KillPlayer(playerName);
        GameObject playerObject = GameObject.Find(playerName);
        GameObject deadPlayer = Instantiate(DeadPlayerPrefab, playerObject.transform.position, Quaternion.identity);
        deadPlayer.name = playerName + " (Dead)";
        DisableKillAbility();
    }
}