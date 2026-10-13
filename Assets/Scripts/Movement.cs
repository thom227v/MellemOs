using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        Walk();
    }

    void Walk()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        Vector3 direction = Vector3.zero;

        if (kb.wKey.isPressed) direction += Vector3.up;
        if (kb.sKey.isPressed) direction += Vector3.down;
        if (kb.aKey.isPressed) direction += Vector3.left;
        if (kb.dKey.isPressed) direction += Vector3.right;

        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}
