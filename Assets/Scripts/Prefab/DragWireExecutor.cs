using UnityEngine;
using UnityEngine.EventSystems;

public class DragWireExecutor : MonoBehaviour, IDragHandler
{
    private float timeCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3? startPos = null;
    public void OnDrag(PointerEventData data)
    {
        if (data.dragging)
        {
            timeCount += Time.deltaTime;
            if (timeCount > 0.05)
            {
                RectTransform rect = GetComponent<RectTransform>();
                BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();

                if (startPos == null)
                {
                    startPos = rect.position;
                }

                Vector3 start = startPos ?? rect.position;
                Vector3 end = data.position;
                Vector3 direction = end - start;

                rect.position = (start + end) / 2f;
                rect.sizeDelta = new Vector2(direction.magnitude, rect.sizeDelta.y);
                boxCollider2D.size = rect.sizeDelta;

                rect.rotation = Quaternion.Euler(
                    0f, 0f,
                    Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg
                );

                timeCount = 0.0f;
            }
        }
    }
}
