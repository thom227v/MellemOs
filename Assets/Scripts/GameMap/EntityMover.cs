using System;
using UnityEngine;

public class EntityMover : MonoBehaviour
{
    private string thisName;
    private (float, float) newPosition = (0f, 0f);
    void Start()
    {
        thisName = gameObject.name;
        Debug.Log("EntityMover script is attached to: " + gameObject.name);

        if (WSGameMap.Instance != null)
        {
            WSGameMap.Instance.OnPlayerPositionReceived += OnPositionChanged;
        }

    }

    void Update()
    {
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            OnMoved();
        }

        if (newPosition != (0f, 0f))
        {
            Debug.Log("Updating position for player: " + thisName + " to new position: (" + newPosition.Item1 + ", " + newPosition.Item2 + ")");
            transform.position = new Vector3(newPosition.Item1, newPosition.Item2, transform.position.z);
            newPosition = (0f, 0f);
            transform.hasChanged = false;
        }
    }

    void OnPositionChanged(PlayerPositionData positionData)
    {
        Debug.Log("Position data received for player: " + positionData.PlayerId + " with position: (" + positionData.PositionX + ", " + positionData.PositionY + ")");
        if (positionData.PlayerId == thisName)
        {
            newPosition = (positionData.PositionX, positionData.PositionY);
        }
        else
        {
            Debug.Log("PositionData does not match this entity's ID. Received: " + positionData.PlayerId + ", Expected: " + gameObject.name);
        }
    }

    void OnMoved()
    {
        WSGameMap.Instance.SendPlayerPosition(new PlayerPositionData
        {
            PlayerId = gameObject.name,
            PositionX = transform.position.x,
            PositionY = transform.position.y
        });
    }
}
