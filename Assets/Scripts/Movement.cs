using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public bool isVoting = false;
    public float speed = 4f;
    public GameObject voteButton;

    void Update()
    {
        Walk();
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
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Inside zone");
        Keyboard kb = Keyboard.current;
        if (other.name == "ReportButton")
        {
            voteButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "ReportButton") voteButton.SetActive(false);
    }
}
