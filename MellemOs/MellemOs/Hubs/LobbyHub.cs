using MellemOs.Models;
using MellemOs.Services;
using Microsoft.AspNetCore.SignalR;

namespace MellemOs.Hubs
{
    public class LobbyHub : Hub
    {
        private LobbyService _lobbyService { get; }

        public LobbyHub(LobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }
        public override async System.Threading.Tasks.Task OnConnectedAsync()
        {
            Console.WriteLine("Dette er en god connection");
            await base.OnConnectedAsync();
        }
        public override async System.Threading.Tasks.Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine("Dette er en god disconnection");
            await base.OnDisconnectedAsync(exception);
        }
        public async System.Threading.Tasks.Task ConnectionTest(string text)
        {
            Console.WriteLine(text);
        }

        public async Task<Lobbies> GetLobbies()
        {
            return await System.Threading.Tasks.Task.FromResult(_lobbyService.GetLobbies());
        }

        public async System.Threading.Tasks.Task JoinLobby(string lobbyName, string username)
        {

            Lobby? lobby = _lobbyService.GetLobbyByName(lobbyName);

            if (lobby != null && lobby.MaxPlayers >= lobby.Players.Count + 1 && lobby.Players.Contains(new Player() { Name = username }) == false)
            {
                lobby.Players.Add(new Player()
                {
                    Name = username
                });
                _lobbyService.ChangeLobby(lobby);
            }
        }

        public async System.Threading.Tasks.Task CreateLobby(string lobbyName, int size)
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
            Lobby? lobby = _lobbyService.GetLobbies().Lobbier.FirstOrDefault(l => l.Players.Any(p => p.Name == playername));
            if (lobby != null)
            {
                return await System.Threading.Tasks.Task.FromResult(lobby.Players.Select(p => p.Name).ToArray());
            }
            else
            {
                return await System.Threading.Tasks.Task.FromResult(Array.Empty<string>());
            }
        }

        public async void StartGame(string lobbyName)
        {
            Lobbies lobbies = _lobbyService.GetLobbies();

            Lobby? lobby = lobbies.Lobbier.FirstOrDefault(l => l.Name == lobbyName);

            if (lobby != null)
            {
                lobby.GameState = Enums.GameState.Started;
                _lobbyService.ChangeLobby(lobby);
            }
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
                    _lobbyService.ChangeLobby(lobby);
                }
            }
        }
    }
}