using System.Collections.Generic;
using System;
using UnityEngine;

public class VotingPanel : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    public List<VotingData> votingDataList = new List<VotingData>();

    public void RenderVotes()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach (var votingData in votingDataList)
        {
            GameObject row = Instantiate(rowPrefab, transform);
            row.GetComponent<VotePanelRowUI>().Setup(votingData);
            //row.GetComponent<VotePanelRowUI>().Setup(votingData);
        }
    }

    public void AddVote(string voterName, string votedAgainstName)
    {
        votingDataList.Add(new VotingData { voterName = voterName, votedAgainstName = votedAgainstName });
        RenderVotes();  
    }
}

[Serializable]
public class VotingData
{
    public string voterName;
    public string votedAgainstName;

}
