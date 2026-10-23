using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject VotingPanelContent;
    [SerializeField] public GameObject VoteOptions;
    [SerializeField] private ScrollRect scrollRect;
    public GameObject player;
    public GameObject votingButton;
    public GameObject votingPanel;
    public GameObject DeadPlayerPrefab;
    public TMP_Text votedStats;
    public GameObject entityPrefab;
    public EntityMover entityMoverScript;

    public GameObject VotingBoard;

    public bool votingStarted = false;
    public Player[] players = null;

    private Dictionary<string, int> voteCounts = null;
    public (string voter, string votedAgainst) newVote = (null, null);
    public string votedOutPlayer = null;
    public string newPlayerDead = null;

    public bool gameFinished = false;
    public bool playerWon = false;

    public static GameManager Instance { get; private set; }

    async void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"));

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

    private void Update()
    {
        if (votingStarted)
        {
            votingStarted = false;
            Vector2 p = Random.insideUnitCircle * 2f;
            player.transform.position = VotingBoard.transform.position + new Vector3(p.x, 0f, p.y);
            player.GetComponent<Movement>().isVoting = true;
            voteCounts = GetEmptyVoteDictioaryFromPlayers(players);
            votingButton.SetActive(false);
            votingPanel.SetActive(true);
            VotingPanelContent.GetComponent<VotingPanel>().votingDataList = new List<VotingData>();
            VotingPanelContent.GetComponent<VotingPanel>().RenderVotes();
            if (player.GetComponent<Movement>().isAlive == true)
            {
                VoteOptions.GetComponent<VoteOptions>().Setup(GetVoteOptions());
            }
            else
            {
                VoteOptions.SetActive(false);
            }
            string text = $"Current Voted:  0/{players.Length}";
            foreach (var pair in voteCounts)
                text += $"\n{pair.Key}: {pair.Value}";
            votedStats.text = text;
        }

        if (newVote.voter != null && newVote.votedAgainst != null)
        {
            VoteRegistered(newVote.voter, newVote.votedAgainst);
            newVote = (null, null);
        }

        if (!string.IsNullOrEmpty(votedOutPlayer))
        {
            Debug.Log($"Player {votedOutPlayer} was voted out.");
            StartCoroutine(HandleVotingFinished(votedOutPlayer));
            votedOutPlayer = null;
        }

        if (gameFinished)
        {
            gameFinished = false;
            FinishGame(playerWon);
        }
        if (!string.IsNullOrEmpty(newPlayerDead))
        {
            PlayerDied(newPlayerDead);
            newPlayerDead = null;
        }
    }

    public static Dictionary<string, int> GetEmptyVoteDictioaryFromPlayers(Player[] players)
    {
        Dictionary<string, int> voteCounts = new();
        foreach (var player in players)
        {
            if (player.IsAlive) voteCounts[player.Name] = 0;
        }
        voteCounts["Skip"] = 0;
        return voteCounts;
    }

    public async void ReportButtonTriggered()
    {
        await WSGameMap.Instance.StartVote();
    }

    public async void FinishGame(bool playersWon)
    {
        if (playersWon)
        {
            Debug.Log("Players win");
            SceneManager.LoadScene("firstscene");
        }
        else
        {
            Debug.Log("Murderers win");
            SceneManager.LoadScene("firstscene");
        }
    }


    public async void VoteButtonTriggered(TMP_Text votedAgainst)
    {
        Debug.Log($"voted against {votedAgainst.text}");
        await WSGameMap.Instance.RegisterVote(votedAgainst.text);
    }

    private void VoteRegistered(string voter, string votedAgainst)
    {
        bool wasAtBottom = IsAtBottom();
        VotingPanelContent.GetComponent<VotingPanel>().AddVote(GameInformation2.username, votedAgainst);
        if (wasAtBottom)
        {
            StartCoroutine(ScrollToBottomNextFrame());
        }

        voteCounts[votedAgainst]++;
        string text = $"Current Voted:  {voteCounts.Values.Sum()}/{players.Length}";
        foreach (var pair in voteCounts)
            text += $"\n{pair.Key}: {pair.Value}";
        votedStats.text = text;

        player.GetComponent<Movement>().hasVoted = true;
        if (voter == GameInformation2.username)
            VoteOptions.SetActive(false);

        Debug.Log("Before 2 sec delay");
        //if (voteCounts.Values.Sum() == players.Length) StartCoroutine(HandleVotingFinished());
    }

    private IEnumerator HandleVotingFinished(string votedOutPlayer)
    {
        yield return new WaitForSeconds(1.5f);

        string text = $"Voting Finished!\n";
        string playerHighestVote = votedOutPlayer;
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

        if (playerHighestVote == GameInformation2.username)
        {
            players.FirstOrDefault(p => p.Name == playerHighestVote).IsAlive = false;
            player.GetComponent<Movement>().isAlive = false;
        }
        else if (playerHighestVote != "Skip" && (playerHighestVote != GameInformation2.username))
        {
            players.FirstOrDefault(p => p.Name == playerHighestVote).IsAlive = false;
            Destroy(GameObject.Find(playerHighestVote));
        }

        player.GetComponent<Movement>().isVoting = false;
        player.GetComponent<Movement>().hasVoted = false;
        VoteOptions.SetActive(true);

        voteCounts = GetEmptyVoteDictioaryFromPlayers(players);
    }
    private List<string> GetVoteOptions()
    {
        List<string> voteOptions = new List<string>();

        foreach (var player in players)
        {
            if (player.Name == GameInformation2.username) continue;
            if (player.IsAlive) voteOptions.Add(player.Name);
        }

        voteOptions.Add("Skip");
        return voteOptions;
    }

    public void PlayerDied(string playerName)
    {
        if (playerName == GameInformation2.username)
        {
            player.GetComponent<Movement>().isAlive = false;
            GameObject deadPlayer = Instantiate(DeadPlayerPrefab, player.transform.position, Quaternion.Euler(0, 0, -90));
            deadPlayer.name = playerName + " (Dead)";
        }
        else
        {
            Player player = players.FirstOrDefault(p => p.Name == playerName);
            if (player != null)
            {
                player.IsAlive = false;
            }
            GameObject playerObject = GameObject.Find(playerName);
            GameObject deadPlayer = Instantiate(DeadPlayerPrefab, playerObject.transform.position, Quaternion.Euler(0, 0, -90));
         deadPlayer.name = playerName + " (Dead)";
            Destroy(playerObject);
        }
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