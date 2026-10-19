using System;
using System.Collections.Generic;
using System.Text;
using MellemOs.Models;

namespace MellemOs.Services;

public class MatchService
{
    List<Match> Matches { get; set; } = new()
    {
        new Match(0),
    };

    public void CreateMatch(int matchId)
    {
        var match = new Match(matchId);
        Matches.Add(match);
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
