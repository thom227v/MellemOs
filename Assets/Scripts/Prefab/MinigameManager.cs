using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class MinigameManager : MonoBehaviour
{
    public Canvas ShapeGame;
    public Canvas ElectricalGame;
    public Canvas RedLightGame;
    public Canvas NumberMinigame;
    public Canvas ColorMinigame;
    public bool minigameOpen = false;
    private Dictionary<int, Canvas> minigames = new Dictionary<int, Canvas>();
    private List<GameTask> tasksToDo = new List<GameTask>();

    public static MinigameManager Instance { get; private set; }

    async void Awake()
    {
        tasksToDo = await WSGameMap.Instance.GetTasksByPlayerName(PlayerPrefs.GetString("username"));

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

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
        // SEND TING TO THE TING TING SERVER BOOM BOOM
        WSGameMap.Instance.CompleteTaskById(PlayerPrefs.GetString("username"), id);
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
}
