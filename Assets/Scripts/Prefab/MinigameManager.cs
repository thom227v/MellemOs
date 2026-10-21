using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using static MinigameManager;

public class MinigameManager : MonoBehaviour
{
    public Canvas ShapeGame;
    public Canvas ElectricalGame;
    public Canvas RedLightGame;
    public Canvas NumberMinigame;
    public Canvas ColorMinigame;
    public GameObject Tasks;
    public TMP_Text RoleExpose;
    public bool minigameOpen = false;
    private Dictionary<int, Canvas> minigames = new Dictionary<int, Canvas>();
    private List<GameTask> tasksToDo = new List<GameTask>();

    public static MinigameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    IEnumerator ShowRole(string roleTitle)
    {
        RoleExpose.text = $"YOU ARE: {roleTitle}";
        RoleExpose.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        RoleExpose.gameObject.SetActive(false);
    }

    private void AddTask(string taskName)
    {
        GameObject text = new GameObject("TaskText");
        TextMeshProUGUI newText = text.AddComponent<TextMeshProUGUI>();
        newText.text = taskName;
        newText.enableAutoSizing = true;
        text.transform.SetParent(Tasks.transform, false);
    }

    public async System.Threading.Tasks.Task Setup()
    {
        string username = GameInformation2.username;
        tasksToDo = await WSGameMap.Instance.GetTasksByPlayerName(username);
        foreach(GameTask task in tasksToDo)
        {
            AddTask(task.Name);

            Debug.Log(task.Name);
        }

        MinigameManager.Role role = await WSGameMap.Instance.GetRoleByPlayerName(username);

        if (role.RoleTitle == RoleEnum.Murderer)
        {
            AddTask("KILL ALL PLAYERS");
        }
        else if(role.RoleTitle == RoleEnum.Detective)
        {
            AddTask("FIND THE MURDERER");
        }

        StartCoroutine(ShowRole(role.RoleTitle.ToString()));

        minigames = new Dictionary<int, Canvas>()
        {
            { 0, ShapeGame },
            { 1, ElectricalGame },
            { 2, RedLightGame },
            { 3, NumberMinigame  },
            { 4, ColorMinigame  }
        };
    }

    public void StartMinigameById(int id)
    {
        GameTask? task = tasksToDo.FirstOrDefault<GameTask>(l => l.Id == id);
        if (task == null) { return; }
        if (task.Completed) { return; }

        if (minigameOpen) { return; }
        if (minigames.ContainsKey(id))
        {
            minigameOpen = true;
            Canvas canvas = Instantiate(minigames[id]);
            IMinigame minigame = canvas.GetComponent<IMinigame>();
            minigame.id = id;
            minigame.minigameManager = this;
            canvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError($"Minigame with ID {id} does not exist.");
        }
    }

    public void CompletedMinigame(int id, GameObject gameObject)
    {
        Debug.Log($"Completed {id}");
        // SEND TING TO THE TUNG TUNG TUNG SAHUR SERVER BOOM BOOM
        WSGameMap.Instance.CompleteTaskById(GameInformation2.username, id);
        GameTask? task = tasksToDo.FirstOrDefault<GameTask>(l => l.Id == id);
        if (task != null)
        {
            task.Completed = true;
        }
        Destroy(gameObject);
        minigameOpen = false;
    }

    public void ClosedMinigame(GameObject gameObject)
    {
        Destroy(gameObject);
        minigameOpen = false;
    }

    public class GameTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; } = false;
    }

    public enum RoleEnum
    {
        Murderer,
        Innocent,
        Detective
    }

    public class Role
    {
        public RoleEnum RoleTitle { get; set; }
        public int AbilityCooldown { get; set; }
    }
}
