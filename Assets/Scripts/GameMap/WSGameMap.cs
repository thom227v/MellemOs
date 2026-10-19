using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WSGameMap : MonoBehaviour
{
    public const string ServerAddress = "http://localhost:5258/";
    private const string Hub = "hubs/match";
    private const int MockMatchId = 0;


    private HubConnection _connection;
    public event Action<PlayerPositionData> OnPlayerPositionReceived;

    public static WSGameMap Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public async Task CreateConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(ServerAddress + Hub + '/' + MockMatchId)
            .WithAutomaticReconnect()
            .Build();

        /*
        _connection.Closed += ex =>
        {
            Debug.LogError($"Closed: {ex}");
            return Task.CompletedTask;
        };
        */

        try
        {
            await _connection.StartAsync();
            Debug.Log("Create: " + _connection.State);

            //await _connection.SendAsync("ConnectionTest", "Completed connection");
        }
        catch (Exception ex)
        {
            Debug.LogError($"SignalR connection failed: {ex}");
        }
    }

    public async Task GetConnectionState()
    {
        if (_connection == null)
        {
            Debug.LogWarning("Connection is not established.");
            return;
        }
        var state = _connection.State;
        Debug.Log($"Current connection state: {state}");
    }

    public async void Start()
    {
        await CreateConnection();
        try
        {
            _connection.On<PlayerPositionData>("GetLobbies", (playerPositionData) =>
            {
                OnPlayerPositionReceived?.Invoke(playerPositionData);
            });
            Debug.Log("Start; " + _connection.State);
        }
        catch (Exception e)
        {
            Debug.Log("error" + e.Message);
        }
    }

    public async void SendPlayerPosition(PlayerPositionData playerPositionData)
    {        
        if (_connection == null || _connection.State != HubConnectionState.Connected)
        {
            Debug.LogWarning("Connection is not established or not connected.");
            return;
        }
        try
        {
            Debug.Log("SendPlayerPosition: " + _connection.State);
            await _connection.SendAsync("BroadCastPlayerPosition", playerPositionData);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send player position: {ex}");
        }
    }
}

public record PlayerPositionData
{
    public string PlayerId { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }
}