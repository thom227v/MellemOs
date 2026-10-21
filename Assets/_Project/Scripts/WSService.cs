using Microsoft.AspNetCore.SignalR.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WSService : MonoBehaviour
{
    public const string ServerAddress = "http://localhost:5258/";
    private const string Hub = "hubs/lobby";
    private HubConnection _connection;
    public event Action<Lobbies> OnLobbiesReceived;

    public static WSService Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public async Task CreateConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(ServerAddress + Hub)
            .WithAutomaticReconnect()
            .Build();

        _connection.Closed += ex =>
        {
            Debug.LogError($"Closed: {ex}");
            return Task.CompletedTask;
        };

        try
        {
            await _connection.StartAsync();
            Debug.Log("Create: " + _connection.State);

            await _connection.SendAsync("ConnectionTest", "Completed connection");
        }
        catch (Exception ex)
        {
            Debug.LogError($"SignalR connection failed: {ex}");
        }
    }

    public async Task<bool> UsernameCheck(string username)
    {
        if (_connection == null)
        {
            Debug.LogWarning("Connection is not established.");
            return false;
        }
        try
        {
            bool isUsernameTaken = _connection.InvokeAsync<bool>("UsernameCheck", username).Result;
            return isUsernameTaken;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to check username: {ex}");
            return false;
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

    public Lobbies GetLobbies()
    {
        if (_connection == null)
        {
            Debug.LogWarning("Connection is not established.");
            return null;
        }
        try
        {
            Lobbies lobbies = _connection.InvokeAsync<Lobbies>("GetLobbies").Result;
            return lobbies;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get lobbies: {ex}");
            return null;
        }
    }

    public Lobbies GetLobbiesNotStarted()
    {
        if (_connection == null)
        {
            Debug.LogWarning("Connection is not established.");
            return null;
        }
        try
        {
            Lobbies lobbies = _connection.InvokeAsync<Lobbies>("GetLobbiesNotStarted").Result;
            return lobbies;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get lobbies: {ex}");
            return null;
        }
    }

    public string GetLobbyName()
    {
        if (_connection == null)
        {
            Debug.LogWarning("Connection is not established.");
            return null;
        }
        try
        {
            var lobbyName = _connection.InvokeAsync<string>("GetLobbyName", GameInformation2.username);
            return lobbyName.Result;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get lobby name: {ex}");
            return null;
        }
    }

    public async void Start()
    {
        await CreateConnection();
        try
        {
            _connection.On<Lobbies>("GetLobbies", (lobbies) =>
            {
                OnLobbiesReceived?.Invoke(lobbies);
            });
            await _connection.StartAsync();
            Debug.Log("Start; " + _connection.State);
        }
        catch (Exception e)
        {
            Debug.Log("error" + e.Message);
        }
    }

    public async void JoinLobby(string lobbyName)
    {
        string username = GameInformation2.username;
        await _connection.InvokeAsync("JoinLobby", lobbyName, username);
    }

    public async void CreateLobby(string lobbyName, int size)
    {
        await _connection.InvokeAsync("CreateLobby", lobbyName, size);
    }

    public async Task<string[]> GetPlayersInCurrentLobby()
    {
        return await _connection.InvokeAsync<string[]>("GetPlayersInCurrentPlayerLobby", GameInformation2.username);
    }

    public async Task<int> StartGame(string lobbyName)
    {
        int matchId = await _connection.InvokeAsync<int>("StartGame", lobbyName);
        return matchId;
    }

    public async void LeaveLobby()
    {
        await _connection.InvokeAsync("LeaveLobby", GameInformation2.username);
    }
}

[Serializable]
public record Lobbies()
{
    public List<Lobby> Lobbier { get; set; } = new List<Lobby>();
}

public record Lobby()
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Player> Players { get; set; } = new List<Player>();
    public int MaxPlayers { get; set; }
    public int Countdown { get; set; }
    public GameState GameState { get; set; }
    public List<Player> VotedPlayers { get; set; } = new List<Player>();
}

public record Player
{
    public string Name { get; set; }
    public bool IsAlive { get; set; } = true;
    public Role Role { get; set; } // possible better ways to handle roles before gameState = started
    public string[] Position { get; set; } = new string[2];
}

public record Role
{
    public string Name { get; set; }
    public int AbilityCooldown { get; set; }
}

public enum GameState
{
    NotStarted = 0,
    InProgress = 1,
    Ended = 2
}