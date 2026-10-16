using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MemoryMinigame : MonoBehaviour
{
    public bool displayText = false;
    public Button closeBtn;
    public GameObject numberButtons;
    private Dictionary<int, Color> standardColors = new();
    private Dictionary<int, Button> numberButtonDict = new();
    private int rowIndex = 0;
    private List<int> numbersInOrder = new List<int>();
    private bool coroutineRunning = false;
    private int MaxNumbers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeBtn.onClick.AddListener(CloseMinigame);
        Button[]  numberButtonArray = numberButtons.GetComponentsInChildren<Button>();
        MaxNumbers = numberButtonArray.Length;

        List<int> numbers = new List<int>();
        for(int i = 1; i < MaxNumbers + 1; i++)
        {
            numbers.Add(i);
        }

        System.Random random = new System.Random();
        foreach(Button button in numberButtonArray)
        {
            int randomNumber = numbers[random.Next(0, numbers.Count - 1)];
            if (displayText)
            {
                button.GetComponentInChildren<TMP_Text>().text = randomNumber.ToString();
            }
            button.onClick.AddListener(() => { NumerBtnCOnClick(randomNumber); });
            numberButtonDict.Add(randomNumber, button);
            standardColors.Add(randomNumber, button.image.color);
            numbers.Remove(randomNumber);
        }

        StartCoroutine(HighlightNumbersCO());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void NumerBtnCOnClick(int number)
    {
        if (coroutineRunning) { return; }

        if (rowIndex == 5)
        {
            Debug.Log("YOU WON");
            return;
        }

        StartCoroutine(CheckAnswerCO(number));
    }

    private IEnumerator CheckAnswerCO(int number)
    {
        if ((numbersInOrder.Count - 1) >= rowIndex && numbersInOrder[rowIndex] == number)
        {
            rowIndex++;
            Button button = numberButtonDict[number];
            button.image.color = Color.green;
            yield return new WaitForSeconds(0.25f);
            button.image.color = standardColors[number];
            yield return new WaitForSeconds(0.25f);

            if (numbersInOrder.Count - 1 == rowIndex - 1)
            {
                StartCoroutine(HighlightNumbersCO());
            }
        }
        else
        {
            numbersInOrder = new List<int>();
            Button button = numberButtonDict[number];
            button.image.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            button.image.color = standardColors[number];
            yield return new WaitForSeconds(0.5f);

            StartCoroutine(HighlightNumbersCO());
        }
    }
    private IEnumerator HighlightNumbersCO()
    {
        if (coroutineRunning == false)
        {
            coroutineRunning = true;
            rowIndex = 0;
            AddRandomNumberToGuess();
            foreach (int number in numbersInOrder)
            {
                yield return new WaitForSeconds(0.5f);
                Button button = numberButtonDict[number];
                button.image.color = Color.yellow;
                yield return new WaitForSeconds(1f);
                button.image.color = standardColors[number];
            }
            coroutineRunning = false;
        }
    }
    void AddRandomNumberToGuess()
    {
        System.Random random = new System.Random();
        numbersInOrder.Add(random.Next(1, MaxNumbers));
    }

    void CloseMinigame()
    {
        DestroyImmediate(gameObject.transform.parent.gameObject, true);
    }
}
