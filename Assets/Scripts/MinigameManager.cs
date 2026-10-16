using UnityEngine;
using System;
using System.Collections.Generic;

public class MinigameManager : MonoBehaviour
{
    public Canvas ShapeGame;
    public Canvas ElectricalGame;
    public Canvas RedLightGame;

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
            { 0, ShapeGame },
            { 1, ElectricalGame },
            { 2, RedLightGame }
        };
    }

    public void StartMinigameById(int id)
    {
        if (minigames.ContainsKey(id))
        {
            minigames[id].gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError($"Minigame with ID {id} does not exist.");
        }
    }

}
