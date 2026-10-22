using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public bool isVoting = false;
    public bool hasVoted = false;
    public float speed = 4f;
    public GameObject voteButton;
    private string thisName;
    private (float, float) newPosition = (0f, 0f);
    public GameObject PlayerPrefab;

    void Start()
    {
        if (WSGameMap.Instance != null)
        {
            WSGameMap.Instance.OnPlayerPositionReceived += OnPositionChanged;
        }
        else
        {
            Debug.LogError("WSGameMap.Instance is null. Cannot subscribe to OnPlayerPositionReceived.");
        }

    }

    void Update()
    {

        if (newPosition != (0f, 0f))
        {
            transform.position = new Vector3(newPosition.Item1, newPosition.Item2, transform.position.z);
            newPosition = (0f, 0f);
            transform.hasChanged = false;
        }

        Walk();
    }

    void OnPositionChanged(PlayerPositionData positionData)
    {
        if (positionData.PlayerId == thisName)
        {
            newPosition = (positionData.PositionX, positionData.PositionY);
        }
        else
        {
            Debug.Log("PositionData does not match this entity's ID. Received: " + positionData.PlayerId + ", Expected: " + gameObject.name);
        }
    }

    void Walk()
    {
        if (isVoting) return;
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        Vector3 direction = Vector3.zero;

        if (kb.wKey.isPressed) direction += Vector3.up;
        if (kb.sKey.isPressed) direction += Vector3.down;
        if (kb.aKey.isPressed) direction += Vector3.left;
        if (kb.dKey.isPressed) direction += Vector3.right;

        transform.Translate(direction.normalized * speed * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            PlayerPositionData positionData = new PlayerPositionData
            {
                PlayerId = GameInformation2.username,
                PositionX = transform.position.x,
                PositionY = transform.position.y
            };
            WSGameMap.Instance.SendPlayerPosition(positionData);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "PlayerCollider") return;

        GameObject targetPlayer = other.transform.root.gameObject;
        if (targetPlayer == gameObject || GameManager.Instance?.players == null) return;

        if (GameManager.Instance.players.Any(p => p.Name == targetPlayer.name))
        {
            Debug.Log("Player is in range of: " + targetPlayer.name);
            RoleManager.Instance.EnableKillAbility(targetPlayer.name);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Keyboard kb = Keyboard.current;
        if (other.name == "ReportButton")
        {
            voteButton.SetActive(true);
        }

        switch (other.name)
        {
            case "ReportButton":
                if (kb.rKey.wasPressedThisFrame)
                {
                    GameManager.Instance.ReportButtonTriggered();
                }
                break;
            case "ShapesMinigame":
                if (kb.eKey.wasPressedThisFrame)
                {
                    MinigameManager.Instance.StartMinigameById(0);
                }
                break;
            case "ElectricalMinigame":
                if (kb.eKey.wasPressedThisFrame)
                {
                    MinigameManager.Instance.StartMinigameById(1);
                }
                break;
            case "RedLightMiniGame":
                if (kb.eKey.wasPressedThisFrame)
                {
                    MinigameManager.Instance.StartMinigameById(2);
                }
                break;
            case "NumberMinigame":
                if (kb.eKey.wasPressedThisFrame)
                {
                    MinigameManager.Instance.StartMinigameById(3);
                }
                break;
            case "ColorMinigame":
                if (kb.eKey.wasPressedThisFrame)
                {
                    MinigameManager.Instance.StartMinigameById(4);
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "ReportButton") voteButton.SetActive(false);
        if (other.name == "PlayerCollider") RoleManager.Instance.DisableKillAbility();
    }
}
