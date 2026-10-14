using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DragMinigame : MonoBehaviour
{
    public Button closeBtn;
    public GameObject shapes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeBtn.onClick.AddListener(CloseMinigame);
    }

    public void CheckCompletions()
    {
        Button[] shapesArray = shapes.GetComponentsInChildren<Button>();

        int completedTasks = 0;
        for (int i = 0; i < shapesArray.Length; i++)
        {
            Button button = shapesArray[i];
            if (button.GetComponent<CollisionDetector>().Completed)
            {
                completedTasks++;
            }
        }
      

        if (completedTasks == shapesArray.Length)
        {
            Debug.Log("TASKS COMPLETED");
        }
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
