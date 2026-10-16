using UnityEngine;
using System;
using System.Collections.Generic;

public class MinigameManager : MonoBehaviour
{
    public Canvas ShapeGame;
    public Canvas ElectricalGame;
    public Canvas RedLightGame;
    public bool minigameOpen = false;
    private Dictionary<int, Canvas> minigames = new Dictionary<int, Canvas>();


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

        minigames = new Dictionary<int, Canvas>()
        {
            { 0, ShapeGame }
            //{ 1, ElectricalGame },
            //{ 2, RedLightGame }
        };
    }

    public void StartMinigameById(int id)
    {
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

    public void CompletedMinigame(int id)
    {
        Debug.Log($"Completed {id}");
        minigameOpen = false;
    }

    public void ClosedMinigame()
    {
        minigameOpen = false;
    }
}
