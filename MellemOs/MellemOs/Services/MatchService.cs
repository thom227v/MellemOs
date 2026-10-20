using System;
using System.Collections.Generic;
using System.Text;
using MellemOs.Models;

namespace MellemOs.Services;

public class MatchService
{
    private int _matchIdCounter = 1;
    List<Match> Matches { get; set; } = new()
    {
        new Match(0, "Default Match", Array.Empty<string>()),
    };

    public void CreateMatch(string lobbyName, string[] playersName)
    {
        var match = new Match(_matchIdCounter++, lobbyName, playersName);
        Matches.Add(match);
        Console.WriteLine($"Match created: {match.Name} with ID: {match.Id}");
    }

    public Match? GetMatchById(int matchId)
    {
        var match = Matches.Find(m => m.Id == matchId);
        if (match == null)
        {
            return null;
        }
        return match;
    }
}
