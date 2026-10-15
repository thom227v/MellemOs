using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject VotingPanelContent;
    [SerializeField] public GameObject VoteOptions;
    [SerializeField] private ScrollRect scrollRect;
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public void ReportButtonTriggered()
    {
        Debug.Log("voting start");
        // Should sende a request to the server to report the button press
    }

    public void VoteButtonTriggered(TMP_Text votedAgainst)
    {
        Debug.Log($"voted against {votedAgainst.text}");
        // Should sende a request to the server to report the vote
        bool wasAtBottom = scrollRect.verticalNormalizedPosition <= 0.9f;
        VotingPanelContent.GetComponent<VotingPanel>().AddVote("Player1", votedAgainst.text);
        if (wasAtBottom)
        {
            scrollRect.verticalNormalizedPosition = 0;
        }

        else
        {
            Debug.Log($"Scroll position: {scrollRect.verticalNormalizedPosition}");
        }
    }
}
