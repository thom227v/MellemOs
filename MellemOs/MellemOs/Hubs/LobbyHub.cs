using MellemOs.Models;
using MellemOs.Services;
using Microsoft.AspNetCore.SignalR;
using System.Formats.Asn1;

namespace MellemOs.Hubs
{
    public class LobbyHub : Hub
    {
        private LobbyService _lobbyService { get; }
        private MatchService _matchService { get; }

        public LobbyHub(LobbyService lobbyService, MatchService matchService)
        {
            _lobbyService = lobbyService;
            _matchService = matchService;
        }
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("Dette er en god connection");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine("Dette er en god disconnection");
            await base.OnDisconnectedAsync(exception);
        }
        public async Task ConnectionTest(string text)
        {
            Console.WriteLine(text);
        }

        public async Task<Lobbies> GetLobbies()
        {
            return _lobbyService.GetLobbies();
        }

        public Lobbies GetLobbiesNotStarted()
        {
            return _lobbyService.GetLobbies().Lobbier.Where(l => l.GameState == Enums.GameState.NotStarted).ToList().Count > 0
                ? new Lobbies()
                {
                    Lobbier = _lobbyService.GetLobbies().Lobbier.Where(l => l.GameState == Enums.GameState.NotStarted).ToList()
                }
                : new Lobbies() { Lobbier = new List<Lobby>() };
        }

        public async Task JoinLobby(string lobbyName, string username)
        {

            Lobby? lobby = _lobbyService.GetLobbyByName(lobbyName);


            if (lobby != null && lobby.MaxPlayers >= lobby.Players.Count + 1)
            {
                string[]? playerNames = lobby!.Players.Select(p => p.Name).ToArray();

                if (playerNames != null && !playerNames.Contains(username))
                {
                    lobby!.Players.Add(new Player()
                    {
                        Name = username
                    });
                }
            }
        }

        public string? GetLobbyName(string username)
        {
            Lobbies lobbies = _lobbyService.GetLobbies();

            string? lobbyName = lobbies.Lobbier.FirstOrDefault(l => l.Players.Any(p => p.Name == username))?.Name;

            return lobbyName;
        }

        public async Task CreateLobby(string lobbyName, int size)
        {
            Lobby newLobby = new Lobby()
            {
                Name = lobbyName,
                MaxPlayers = size,
                GameState = Enums.GameState.NotStarted
            };
            _lobbyService.AddLobby(newLobby);
        }

        public async Task<string[]> GetPlayersInCurrentPlayerLobby(string playername)
        {
            Lobbies lobbies = _lobbyService.GetLobbies();
            Lobby? lobby = lobbies.Lobbier.FirstOrDefault(l => l.Players.Any(p => p.Name == playername));
            if (lobby != null)
            {
                return await Task.FromResult(lobby.Players.Select(p => p.Name).ToArray());
            }
            else
            {
                return await Task.FromResult(Array.Empty<string>());
            }
        }

        public async Task<int?> StartGame(string lobbyName)
        {
            Lobbies lobbies = _lobbyService.GetLobbies();

            Lobby? lobby = lobbies.Lobbier.FirstOrDefault(l => l.Name == lobbyName);

            if (lobby != null)
            {
                lobby.GameState = Enums.GameState.Started;
                int matchId = _matchService.CreateMatch(lobby.Name, lobby.Players.Select(p => p.Name).ToArray());
                return matchId;

                //_lobbyService.ChangeLobby(lobby);
            }

            return null;
        }

        public async Task<bool> UsernameCheck(string username)
        {
            Lobbies lobbies = _lobbyService.GetLobbies();
            bool isUsernameTaken = lobbies.Lobbier.Any(l => l.Players.Any(p => p.Name == username));
            return isUsernameTaken;
        }

        public async void LeaveLobby(string username)
        {
            Lobbies lobbies = _lobbyService.GetLobbies();

            Lobby? lobby = lobbies.Lobbier.FirstOrDefault(l => l.Players.Any(p => p.Name == username));

            if (lobby != null)
            {
                Player? player = lobby.Players.FirstOrDefault(p => p.Name == username);
                if (player != null)
                {
                    lobby.Players.Remove(player);
                    //_lobbyService.ChangeLobby(lobby);
                }
            }
        }
    }
}
