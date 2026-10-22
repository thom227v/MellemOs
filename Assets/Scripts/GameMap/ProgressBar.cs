using TMPro;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public TMP_Text Tasks;
    private bool TaskProgressUpdated = false;
    private string CurrentTaskProgress;
    void Start()
    {
        if (WSGameMap.Instance != null)
        {
            WSGameMap.Instance.TasksProgress += UpdateProgress;
        }
        else
        {
            Debug.LogError("WSGameMap.Instance is null. Cannot subscribe to OnPlayerPositionReceived.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (TaskProgressUpdated)
        {
            Tasks.text = CurrentTaskProgress;
            TaskProgressUpdated = false;
        }
    }

    private void UpdateProgress(int completed, int total)
    {
        CurrentTaskProgress = $"{completed}/{total}";
        TaskProgressUpdated = true;
    }
}
