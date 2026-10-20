using System;
using System.Collections.Generic;
using System.Text;

namespace MellemOs.Models;

public class Match
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Player[] Players { get; set; } = Array.Empty<Player>();

    public Match(int id, string name, string[] playersName)
    {
        Id = id;
        Name = name;

        List<Player> players = new List<Player>();
        foreach (var playerName in playersName)
        {
            players.Add(new Player() { Name = playerName });
        }
        Players = players.ToArray();
    }
}