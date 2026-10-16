using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Create GameObject1 that falls under gravity.  It will pass through
// Example2 and cause a collision.  GameObject1 is moved back to
// the start position and it will again start to fall under gravity.

public class CollisionDetector : MonoBehaviour
{
    public bool Completed;
    public Button ColliderGoal;
    public DragMinigame Minigame;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == ColliderGoal.name)
        {
            Completed = true;
            Minigame.CheckCompletions();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == ColliderGoal.name)
        {
            Completed = false;
        }
    }
}