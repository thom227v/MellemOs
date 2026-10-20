using UnityEngine;

public class EntityMover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("EntityMover script is attached to: " + gameObject.name);

    }

    void Update()
    {
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            OnMoved();
        }

 
        if (WSGameMap.Instance != null)
        {
            WSGameMap.Instance.OnPlayerPositionReceived += OnPositionChanged;
        }
    }

    void OnPositionChanged(PlayerPositionData positionData)
    {
        if (positionData.PlayerId == gameObject.name)
        {
            transform.position = new Vector3(positionData.PositionX, positionData.PositionY, transform.position.z);
        }
        // Handle position change
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
