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

        public async System.Threading.Tasks.Task JoinLobby(Lobby lobbyToJoin, string username)
        {

            Lobby? lobby = _lobbyService.GetLobbyById(lobbyToJoin.Id);

            if (lobby != null && lobby.MaxPlayers >= lobby.Players.Count + 1)
            {
                lobbyToJoin.Players.Add(new Player()
                {
                    Name = username
                });
            }
        }

        public async System.Threading.Tasks.Task CreateLobby(string lobbyName, string username)
        {
            Player player = new Player()
            {
                Name = username
            };
            Lobby newLobby = new Lobby()
            {
                Name = lobbyName,
                Players = [player],
                GameState = Enums.GameState.NotStarted
            };
            _lobbyService.AddLobby(newLobby);
        }
    }
}