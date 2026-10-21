using System;
using System.Collections.Generic;
using System.Text;
using MellemOs.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MellemOs.Services;

public class MatchService
{
    private readonly TaskService _taskService;
    public MatchService(TaskService taskService)
    {
        _taskService = taskService;
    }

    private int _matchIdCounter;
    private List<Match> Matches { get; set; } = new();

    public int CreateMatch(string lobbyName, string[] playersName)
    {
        var match = new Match(_matchIdCounter, lobbyName, playersName);
        _matchIdCounter++;

        foreach(Player player in match.Players)
        {
            player.TasksToDo = _taskService.GenerateRndTasks();
        }

        Matches.Add(match);
        Console.WriteLine($"Match created: {match.Name} with ID: {match.Id}");
        return match.Id;
    }

    public int GetMatchIdByPlayerName(string playerName)
    {
        Match? match = Matches.Find(m => m.Players.Any(p => p.Name == playerName));
        if (match == null)
        {
            throw new NotImplementedException();
        }
        return match.Id;
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

    public List<GameTask> GetTasksByPlayer(int matchId, string playerName)
    {
        Match? match = Matches.Find(m => m.Id == matchId);
        if (match == null) { throw new NotImplementedException(); }
        Player? player = match.Players.FirstOrDefault(l => l.Name == playerName);
        if (player == null) { throw new NotImplementedException(); }
        return player.TasksToDo;
    }

    public void CompleteTaskByPlayer(int matchId, string playerName, int taskId)
    {
        Match? match = Matches.Find(m => m.Id == matchId);
        if (match == null) { throw new NotImplementedException(); }
        Player? player = match.Players.FirstOrDefault(l => l.Name == playerName);
        if (player == null) { throw new NotImplementedException(); }
        GameTask? task = player.TasksToDo.FirstOrDefault(l => l.Id == taskId);
        if (task != null)
        {
            task.Completed = true;
        }
    }
}
