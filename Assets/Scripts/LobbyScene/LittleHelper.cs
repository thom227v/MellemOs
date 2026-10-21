using UnityEngine;
using UnityEngine.SceneManagement;

public class LittleHelper : MonoBehaviour
{
    public bool lobbyStarted = false;
    public static LittleHelper Instance;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lobbyStarted)
        {
            SceneManager.LoadScene("GAMEMAP");
        }
    }
}
