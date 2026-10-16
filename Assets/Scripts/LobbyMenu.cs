using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private Button button;

    public void Setup(string textValue)
    {
        text.text = textValue;
    }
}
