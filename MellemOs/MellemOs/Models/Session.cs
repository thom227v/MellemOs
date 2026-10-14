namespace MellemOs.Models
{
    public class Session
    {
        public List<Player> Players { get; set; } = [];
        public int MaxPlayers { get; set; }
        public int Countdown { get; set; }
        public required GameState GameState { get; set; }
        public List<Player> VotedPlayers { get; set; } = [];
    }
}
