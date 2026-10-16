using MellemOs.Models;
using Microsoft.AspNetCore.SignalR;

namespace MellemOs.Hubs
{
    public class LobbyHub : Hub
    {
        private Lobbies _lobbies;
        public LobbyHub(Lobbies lobbies)
        {
            _lobbies = lobbies;
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
            return await System.Threading.Tasks.Task.FromResult(new Lobbies() { Lobbier = { new Lobby() { Name = "My Lobby", GameState = new GameState() { Type = "My Game" } } } });
        }

        public async System.Threading.Tasks.Task JoinLobby(Lobby lobbyToJoin, string username)
        {
            Player player = new Player()
            { 
                Name = username
            };
            Lobby? lobby = _lobbies.Lobbier?.FirstOrDefault(l => l.Id == lobbyToJoin.Id);

            if (lobby != null && lobby.MaxPlayers >= lobby.Players.Count + 1)
            {
                lobby.Players.Add(player);
            }
        }
    }
}