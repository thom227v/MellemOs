using System.Collections.Generic;
using UnityEngine;

public class TestingHelper : MonoBehaviour
{
    // Voting test
    public GameObject votingPanel;
    public GameObject votingContent;
    public GameObject votingButton;
    public GameObject VoteOptions;
    public GameObject player;
    public void ReportTriggered()
    {
        player.GetComponent<Movement>().isVoting = true;
        votingButton.SetActive(false);
        votingPanel.SetActive(true);
        votingContent.GetComponent<VotingPanel>().votingDataList = GetMockVoteData();
        votingContent.GetComponent<VotingPanel>().RenderVotes();
        VoteOptions.GetComponent<VoteOptions>().Setup(GetMockVoteOptions());
    }

    private List<VotingData> GetMockVoteData()
    {
        List<VotingData> mockDataList = new List<VotingData>();
        VotingData mockData = new VotingData();
        mockData.voterName = "Player1";
        mockData.votedAgainstName = "Player2";
        mockDataList.Add(mockData);
        mockData.voterName = "Player2";
        mockData.votedAgainstName = "Player3";
        mockDataList.Add(mockData);
        mockData.voterName = "Player3";
        mockData.votedAgainstName = "Player2";
        mockDataList.Add(mockData);
        return mockDataList;
    }

    private List<string> GetMockVoteOptions()
    {
        List<string> mockOptions = new List<string>();
        mockOptions.Add("Player1");
        mockOptions.Add("Player2");
        mockOptions.Add("Player3");
        return mockOptions;
    }
}
