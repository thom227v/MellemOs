using Microsoft.AspNetCore.SignalR.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WSGameMap : MonoBehaviour
{
    public const string ServerAddress = "http://localhost:5258/";
    //public const string ServerAddress = "http://192.168.20.5:7235/";
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
            _connection.On<PlayerPositionData>("PlayerPosition", (playerPositionData) =>
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
            //Debug.Log("SendPlayerPosition: " + _connection.State);
            await _connection.SendAsync("BroadCastPlayerPosition", playerPositionData);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send player position: {ex}");
        }
    }

    public async void CompleteTaskById(string playerName, int id)
    {
        if (_connection == null || _connection.State != HubConnectionState.Connected)
        {
            Debug.LogWarning("Connection is not established or not connected.");
            return;
        }
        try
        {
            //Debug.Log("SendPlayerPosition: " + _connection.State);
            await _connection.SendAsync("CompleteTaskByPlayerName", playerName, id);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send player position: {ex}");
        }
    }

    public async Task<List<MinigameManager.GameTask>> GetTasksByPlayerName(string playerName)
    {
        if (_connection == null || _connection.State != HubConnectionState.Connected)
        {
            Debug.LogWarning("Connection is not established or not connected.");
            return new List<MinigameManager.GameTask>();
        }
        try
        {
            //Debug.Log("SendPlayerPosition: " + _connection.State);
            List<MinigameManager.GameTask> listOfTasks = await _connection.InvokeAsync<List<MinigameManager.GameTask>>("GetTasksByPlayerName", playerName);
            return listOfTasks;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send player position: {ex}");
        }
        return new List<MinigameManager.GameTask>();
    }

    public async Task<Player[]> GetPlayers()
    {
        if (_connection == null || _connection.State != HubConnectionState.Connected)
        {
            Debug.LogWarning("Connection is not established or not connected.");
            return null;
        }
        try
        {
            var players = await _connection.InvokeAsync<Player[]>("GetPlayers");
            return players;

        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get all players: {ex}");
        }
        return null;
    }


}

public record PlayerPositionData
{
    public string PlayerId { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }
}