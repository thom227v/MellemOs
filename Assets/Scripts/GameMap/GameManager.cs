using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject VotingPanelContent;
    [SerializeField] public GameObject VoteOptions;
    [SerializeField] private ScrollRect scrollRect;
    public GameObject player;
    public GameObject votingButton;
    public GameObject votingPanel;
    public TMP_Text votedStats;
    public GameObject entityPrefab;
    public EntityMover entityMoverScript;

    public Player[] players = null;
    private Dictionary<string, int> voteCounts = null;
    public static Dictionary<string, int> GetEmptyVoteDictioaryFromPlayers(Player[] players)
    {
        Dictionary<string, int> voteCounts = new();
        foreach (var player in players)
        {
            voteCounts[player.Name] = 0;
        }
        voteCounts["Skip"] = 0;
    return voteCounts;
    }


    public static GameManager Instance { get; private set; }

    async void Start()
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

        Application.runInBackground = true;
        await WSGameMap.Instance.Setup();

        if (WSGameMap.Instance == null) Debug.Log("WSGameMap.Instance is null");
        Debug.Log("Getting players from WSGameMap.Instance");
        Player[]? players = await WSGameMap.Instance.GetPlayers();
        if (players == null || players.Length == 0)
        {
            Debug.LogError("No players found.");
            return;
        }
        this.players = players;

        foreach (var player in players)
        {
            if (player.Name == GameInformation2.username) continue;
            Debug.Log("username: " + GameInformation2.username + " player.Name: " + player.Name);
            Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            CreateEntity(player.Name, randomPosition);
        }
        await MinigameManager.Instance.Setup();
    }

    public void ReportButtonTriggered()
    {
        player.GetComponent<Movement>().isVoting = true;
        votingButton.SetActive(false);
        votingPanel.SetActive(true);
        VotingPanelContent.GetComponent<VotingPanel>().votingDataList = new List<VotingData>();
        VotingPanelContent.GetComponent<VotingPanel>().RenderVotes();
        VoteOptions.GetComponent<VoteOptions>().Setup(GetVoteOptions());
        string text = $"Current Voted:  0/{players.Length}";

        foreach (var pair in voteCounts)
            text += $"\n{pair.Key}: {pair.Value}";

        votedStats.text = text;
    }

    public void VoteButtonTriggered(TMP_Text votedAgainst)
    {
        Debug.Log($"voted against {votedAgainst.text}");

        bool wasAtBottom = IsAtBottom();
        VotingPanelContent.GetComponent<VotingPanel>().AddVote("Player1", votedAgainst.text);
        if (wasAtBottom)
        {
            StartCoroutine(ScrollToBottomNextFrame());
        }

        voteCounts[votedAgainst.text]++;
        string text = $"Current Voted:  {voteCounts.Values.Sum()}/{players.Length}";
        foreach (var pair in voteCounts)
            text += $"\n{pair.Key}: {pair.Value}";
        votedStats.text = text;

        player.GetComponent<Movement>().hasVoted = true;
        VoteOptions.SetActive(false);

        //if (voteCounts.Values.Sum() == players.Length) HandleVotingFinished();
        Debug.Log("Before 2 sec delay");
        if (voteCounts.Values.Sum() == 1) StartCoroutine(HandleVotingFinished());


    }

    private IEnumerator HandleVotingFinished()
    {
        yield return new WaitForSeconds(1.5f);

        string text = $"Voting Finished!\n";
        string playerHighestVote = voteCounts.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        if (playerHighestVote == "Skip")
        {
            text += "No one was voted out.";
        }
        else
        {
            text += $"Player {playerHighestVote} was voted out.";
        }
        votedStats.text = text;
        yield return new WaitForSeconds(2f);

        votingPanel.SetActive(false);
        player.GetComponent<Movement>().isVoting = false;
        player.GetComponent<Movement>().hasVoted = false;
        VoteOptions.SetActive(true);
        if (playerHighestVote != "Skip") players = players.Where(p => p.Name != playerHighestVote).ToArray();
        voteCounts = GetEmptyVoteDictioaryFromPlayers(players);

    }
    private List<string> GetVoteOptions()
    {
        List<string> mockOptions = new List<string>();

        foreach (var player in players)
        {
            mockOptions.Add(player.Name);
        }
        return mockOptions;
    }

    private bool IsAtBottom()
    {
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport != null
            ? scrollRect.viewport
            : (RectTransform)scrollRect.transform;

        if (content.rect.height <= viewport.rect.height)
            return true;

        return scrollRect.verticalNormalizedPosition <= 0.01f;
    }

    private IEnumerator ScrollToBottomNextFrame()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    private void CreateEntity(string entityName, Vector3 position)
    {
        GameObject entity = Instantiate(entityPrefab, position, Quaternion.identity);
        entity.name = entityName;
        entity.AddComponent<EntityMover>();
    }
}