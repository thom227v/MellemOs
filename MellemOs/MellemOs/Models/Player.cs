namespace MellemOs.Models
{
    public class Player
    {
        public required string Name { get; set; }
        public bool IsAlive { get; set; } = true;
        public Role? Role { get; set; } // possible better ways to handle roles before gameState = started
        public string[] Position { get; set; } = [];
        public GameTask? CurrentTask { get; set; }
        public List<GameTask> TasksToDo { get; set; } = [];
    }
}
