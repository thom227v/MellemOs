using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VoteOptions : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    public void Setup(List<string> options)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach (var option in options)
        {
            GameObject button = Instantiate(buttonPrefab, transform);
            button.GetComponentInChildren<TMP_Text>().text = option;
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GameManager.Instance.VoteButtonTriggered(button.GetComponentInChildren<TMP_Text>()));
        }

    }

    public void AddVoteMessage(string voter, string votedAgainst)
    {
        GameObject button = Instantiate(buttonPrefab);
        button.transform.SetParent(transform);
        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GameManager.Instance.VoteButtonTriggered(button.GetComponentInChildren<TMP_Text>()));

        button.GetComponentInChildren<TMP_Text>().text = $"{voter} voted against {votedAgainst}";
    }
}
