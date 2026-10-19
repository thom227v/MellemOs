using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RedLightMinigame : MonoBehaviour, IMinigame
{
    public int id { get; set; }
    public MinigameManager minigameManager { get; set; }
    public Button closeBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeBtn.onClick.AddListener(CloseMinigame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompletedMinigame()
    {
        minigameManager.CompletedMinigame(id);
        Destroy(gameObject);
    }

    void CloseMinigame()
    {
        minigameManager.ClosedMinigame(gameObject);
    }
}
