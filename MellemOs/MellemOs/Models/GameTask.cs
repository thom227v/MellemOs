namespace MellemOs.Models
{
    public class GameTask
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required bool Completed { get; set; } = false;
    }
}
