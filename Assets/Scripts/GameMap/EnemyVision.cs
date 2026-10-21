
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    private void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject target in players)
        {
            Debug.Log(target.name);
            // Skip yourself
            if (target == gameObject)
                continue;

            Vector2 origin = transform.position;
            Vector2 destination = target.transform.position;
            Vector2 direction = destination - origin;
            float distance = direction.magnitude;

            RaycastHit2D[] hits = Physics2D.RaycastAll(
                origin,
                direction.normalized,
                distance
            );

            bool canSee = true;

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.transform.root == transform.root)
                    continue;

                if (hit.collider.CompareTag("Wall") ||
                    hit.collider.transform.root.CompareTag("Wall"))
                {
                    canSee = false;
                    break;
                }
            }

            Debug.Log(canSee.ToString());
            SpriteRenderer[] sprites = target.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer s in sprites)
            {
                s.enabled = canSee;
            }
        }
    }
}
