using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject UsernameMenu;
    public GameObject PlayGameButton;

    void Start()
    {
        PlayGameButton.GetComponentInChildren<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        PlayGameButton.SetActive(false);
        UsernameMenu.SetActive(true);
    }


    void Update()
    {
        
    }
}