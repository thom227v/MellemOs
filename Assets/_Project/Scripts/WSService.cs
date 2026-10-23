using Microsoft.AspNetCore.SignalR.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WSService : MonoBehaviour
{
    //public const string ServerAddress = "http://localhost:5258/";
    public const string ServerAddress = "http://192.168.20.5:7235/";
    private const string Hub = "hubs/lobby";
    private HubConnection _connection;
    public event Action<Lobbies> OnLobbiesReceived;
    public event Action<string[]> LobbyPlayersRecieved;
    public event Action<string, int> OnGameStarted;

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

        _connection.On("GameStarted", async (string lobbyName, int matchId) =>
        {
            Debug.Log("Game started event received.");
            OnGameStarted?.Invoke(lobbyName, matchId);
        });

        _connection.On("LobbyPlayers", async (string[] players) =>
        {
            LobbyPlayersRecieved?.Invoke(players);
        });
        _connection.On("LobbiesNotStarted", async (Lobbies lobbies) =>
        {
            OnLobbiesReceived?.Invoke(lobbies);
        });

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
            bool isUsernameTaken = await _connection.InvokeAsync<bool>("UsernameCheck", username);
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

    public async Task<Lobbies> GetLobbies()
    {
        if (_connection == null)
        {
            Debug.LogWarning("Connection is not established.");
            return null;
        }
        try
        {
            Lobbies lobbies = await _connection.InvokeAsync<Lobbies>("GetLobbies");
            return lobbies;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get lobbies: {ex}");
            return null;
        }
    }

    public async Task<Lobbies> GetLobbiesNotStarted()
    {
        if (_connection == null)
        {
            Debug.LogWarning("Connection is not established.");
            return null;
        }
        try
        {
            Lobbies lobbies = await _connection.InvokeAsync<Lobbies>("GetLobbiesNotStarted");
            return lobbies;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get lobbies: {ex}");
            return null;
        }
    }

    public async Task<string> GetLobbyName()
    {
        if (_connection == null)
        {
            Debug.LogWarning("Connection is not established.");
            return null;
        }
        try
        {
            string lobbyName = await _connection.InvokeAsync<string>("GetLobbyName", GameInformation2.username);
            return lobbyName;
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
        GetLobbies();
    }

    public async Task JoinLobby(string lobbyName)
    {
        string username = GameInformation2.username;
        await _connection.InvokeAsync("JoinLobby", lobbyName, username);
    }

    public async Task CreateLobby(string lobbyName, int size)
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

public enum RoleEnum
{
    Murderer,
    Innocent,
    Detective
}

public record Role
{
    public RoleEnum RoleTitle { get; set; }
    public int AbilityCooldown { get; set; }
}

public enum GameState
{
    NotStarted = 0,
    InProgress = 1,
    Ended = 2
}