using System;
using UnityEngine;
using UnityEngine.UI;

public class JoinGame : MonoBehaviour
{
    public GameObject Menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponentInChildren<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        Menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
