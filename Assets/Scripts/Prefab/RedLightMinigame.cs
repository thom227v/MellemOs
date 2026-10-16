using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RedLightMinigame : MonoBehaviour
{
    public Button closeBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeBtn.onClick.AddListener(CloseMinigame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CloseMinigame()
    {
        DestroyImmediate(gameObject.transform.parent.gameObject, true);
    }
}
