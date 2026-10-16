using System;
using UnityEngine;
using UnityEngine.UI;

public class FrontPagePlayGame : MonoBehaviour
{
    public GameObject UsernameMenu;
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        UsernameMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
