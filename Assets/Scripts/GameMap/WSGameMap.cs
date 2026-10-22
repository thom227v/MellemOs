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
    private int MatchId = GameInformation2.matchId;

    private HubConnection _connection;
    public event Action<PlayerPositionData> OnPlayerPositionReceived;
    public event Action<int, int> TasksProgress;

    public static WSGameMap Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public async Task Setup()
    {
        await CreateConnection();
        try
        {
            _connection.On<PlayerPositionData>("PlayerPosition", (playerPositionData) =>
            {
                //Debug.Log("Invoked!!!");
                OnPlayerPositionReceived?.Invoke(playerPositionData);
            });
            _connection.On<int, int>("TotalTasks", (int completed, int total) =>
            {
                TasksProgress?.Invoke(completed, total);
            });


            Debug.Log("Start; " + _connection.State);
        }
        catch (Exception e)
        {
            Debug.Log("error" + e.Message);
        }
    }

    public async Task KillPlayer(string playerName)
    {
        if (_connection == null)
        {
            Debug.LogWarning("Connection is not established.");
        }
        try
        {
            List<Player> players = await _connection.InvokeAsync<List<Player>>("KillPlayer", playerName);
            Debug.Log("player killed: " + playerName);
            GameManager.Instance.players = players.ToArray();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to kill player: {ex}");
        }
    }


    public async Task CreateConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(ServerAddress + Hub + '/' + MatchId)
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

    public async Task<Role> GetRoleByPlayerName(string playerName)
    {
        if (_connection == null || _connection.State != HubConnectionState.Connected)
        {
            Debug.LogWarning("Connection is not established or not connected.");
            throw new Exception("NO Connection");
        }
        try
        {
            //Debug.Log("SendPlayerPosition: " + _connection.State);
            Role role = await _connection.InvokeAsync<Role>("GetRoleByPlayerName", playerName);
            return role;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send player position: {ex}");
        }
        throw new Exception("NO ROLE");
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
            Debug.Log("GetPlayers: " + _connection.State);
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