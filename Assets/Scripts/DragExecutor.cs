using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragExecutor : MonoBehaviour, IDragHandler
{
    public Button theButton;
    private float timeCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.dragging)
        {
            timeCount += Time.deltaTime;
            if (timeCount > 0.05)
            {
                //Debug.Log("Dragging:" + data.position);
                theButton.transform.position = data.position;
                timeCount = 0.0f;
            }
        }
    }
}
