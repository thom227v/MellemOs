namespace MellemOs.Models
{
    public class VotedPlayer
    {
        public required Player Player { get; set; }
        public required Player VotedByPlayer { get; set; }
    }
}
