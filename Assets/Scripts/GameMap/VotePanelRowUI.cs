using TMPro;
using UnityEngine;

public class VotePanelRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text voterText;
    [SerializeField] private TMP_Text actionText;
    [SerializeField] private TMP_Text votedAgainstText;
    
    public void Setup(VotingData votingData)
    {
        voterText.text = votingData.voterName;
        actionText.text = "voted against";
        votedAgainstText.text = votingData.votedAgainstName;
    }
}
