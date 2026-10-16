using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class BroadcastService : MonoBehaviour, IInitializable
{

    public const string ServerAddress = "https://localhost:7235/";
    private const string Hub = "/NotificationHub";
    public static BroadcastService inst;
    public List<string> ReceivedMessages = new List<string>();

    private HubConnection _connection;
    public event Action<string> OnMessageReceived;

  
    void Awake()
    {
        inst = this;
    }

    void Start()
    {
        Initialize();
        OnMessageReceived += MessageReceived;
        StartCoroutine(LookForTasksInMessagesCO());
    }
    IEnumerator LookForTasksInMessagesCO()
    {
        while (true)
        {

            if (ReceivedMessages.Count>0)
            {
                StartCoroutine(TakeOrderCO(ReceivedMessages[0]));
                ReceivedMessages.RemoveAt(0);
            }

            yield return null;
        }
    }

    void Update() {
        if (Input.GetKeyUp("h"))
        {
            BroadcastMessage("Hej Med jer!");
        }
                
    }

    void MessageReceived(string message)
    {
        ReceivedMessages.Add(message);
    }



    IEnumerator TakeOrderCO(string message)
    {
        var password = message.Split(",")[0];
        var order = message.Split(",")[1];
        yield return null;
    }

    public async void Initialize()
    {
        _connection = new HubConnectionBuilder().WithUrl(ServerAddress+Hub).Build();
        _connection.On("OnMessageReceived", (string message) => {
            OnMessageReceived?.Invoke(message);
        });
        await _connection.StartAsync();
        Debug.Log(_connection.State);
    }

    public async Task BroadcastMessage(string message)
    {
        await _connection.SendAsync("BroadcastMessage",message);
    }
}
