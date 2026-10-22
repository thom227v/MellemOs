namespace MellemOs.Models;

public class Match
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Player[] Players { get; set; }
    public Dictionary<string, int> Votes { get; set; } = [];

    public Match(int id, string name, string[] playersName)
    {
        Id = id;
        Name = name;

        List<Player> players = [];
        foreach (var playerName in playersName)
        {
            players.Add(new Player() { Name = playerName });
        }
        Players = players.ToArray();
    }
}
