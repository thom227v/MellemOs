using UnityEngine;

public interface IMinigame
{
    int id { get; set; }
    MinigameManager minigameManager { get; set; }
}
