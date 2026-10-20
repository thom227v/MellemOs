namespace MellemOs.Models
{
    public class GameTask
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public bool Completed { get; set; } = false;
    }
}
