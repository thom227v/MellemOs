using MellemOs.Models;

namespace MellemOs.Services
{
    public class LobbyService
    {
        public Lobbies lobbies { get; set; } = new();

        public Lobbies GetLobbies()
        {
            return lobbies;
        }

        public void ChangeLobby(Lobby lobby)
        {
            var existingLobby = lobbies.Lobbier.FirstOrDefault(l => l.Id == lobby.Id);
            if (existingLobby != null)
            {
                existingLobby.Name = lobby.Name;
                existingLobby.Players = lobby.Players;
                existingLobby.MaxPlayers = lobby.MaxPlayers;
                existingLobby.Countdown = lobby.Countdown;
                existingLobby.GameState = lobby.GameState;
                existingLobby.VotedPlayers = lobby.VotedPlayers;
            }
        }

        public void AddLobby(Lobby lobby)
        {
            lobbies.Lobbier.Add(lobby);
        }

        public Lobby? GetLobbyById(int lobbyId)
        {
            return lobbies.Lobbier.FirstOrDefault(l => l.Id == lobbyId);
        }
    }
}