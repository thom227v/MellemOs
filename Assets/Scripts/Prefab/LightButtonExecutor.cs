using TMPro;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;

public class LightButtonExecutor : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Coroutine coroutine = null;
    private DateTime timerEnded;
    private bool Completed = false;
    private IEnumerator StartRandomTimerCO(Button button)
    {
        System.Random rnd = new();
        int waitForTime = rnd.Next(2000, 10000);
        yield return new WaitForSeconds(waitForTime / 1000f);
        button.image.color = Color.yellow;
        button.GetComponentInChildren<TMP_Text>().text = "STOP";
        timerEnded = DateTime.Now;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Completed || coroutine != null) { return; }
        Button button = gameObject.GetComponent<Button>();
        button.image.color = Color.green;
        button.GetComponentInChildren<TMP_Text>().text = "KEEP HOLDING";

        coroutine = StartCoroutine(StartRandomTimerCO(button));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Completed) { return; }
        Button button = gameObject.GetComponent<Button>();
        if ((DateTime.Now - timerEnded).TotalMilliseconds < 300)
        {
            button.image.color = Color.green;
            button.GetComponentInChildren<TMP_Text>().text = "WON";
            Completed = true;
            // REPORT TO TASK HERE
        }
        else
        {
            button.image.color = Color.red;
            button.GetComponentInChildren<TMP_Text>().text = "YOU FAILED! HOLD ME TO TRY AGAIN";
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}
