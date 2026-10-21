using MellemOs.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MellemOs.Services;

public class MatchService
{
    private readonly TaskService _taskService;
    public MatchService(TaskService taskService)
    {
        _taskService = taskService;
    }

    private void AssignRoles(ref Models.Match match)
    {
        System.Random rnd = new System.Random();

        Player murdererPlayer = match.Players[rnd.Next(0, match.Players.Length)];
        murdererPlayer.Role = new Role { RoleTitle = RoleEnum.Murderer, AbilityCooldown = 15 };

        Player detectivePlayer = match.Players[rnd.Next(0, match.Players.Length)];
        detectivePlayer.Role = new Role { RoleTitle = RoleEnum.Detective, AbilityCooldown = 15 };

        foreach (Player player in match.Players)
        {
            if (murdererPlayer.Name == player.Name) { continue; }
            if (detectivePlayer.Name == player.Name) { continue; }

            player.TasksToDo = _taskService.GenerateRndTasks();
            player.Role = new Role { RoleTitle = RoleEnum.Innocent, AbilityCooldown = 15 };
        }
    }

    private int _matchIdCounter;
    private List<Models.Match> Matches { get; set; } = new();

    public int CreateMatch(string lobbyName, string[] playersName)
    {
        Models.Match match = new Models.Match(_matchIdCounter++, lobbyName, playersName);

        AssignRoles(ref match);

        Matches.Add(match);
        Console.WriteLine($"Match created: {match.Name} with ID: {match.Id}");
        return match.Id;
    }

    public int GetMatchIdByPlayerName(string playerName)
    {
        Models.Match? match = Matches.Find(m => m.Players.Any(p => p.Name == playerName));
        if (match == null)
        {
            throw new NotImplementedException();
        }
        return match.Id;
    }

    public Models.Match? GetMatchById(int matchId)
    {
        return Matches.Find(m => m.Id == matchId);
    }

    public Player? GetPlayerInMatch(Models.Match? match, string playerName)
    {
        if (match == null) return null;
        return match.Players.FirstOrDefault(l => l.Name == playerName);
    }

    public List<GameTask> GetTasksByPlayer(int matchId, string playerName)
    {
        Models.Match? match = GetMatchById(matchId);
        if (match == null) { throw new NotImplementedException(); }
        Player? player = match.Players.FirstOrDefault(l => l.Name == playerName);
        if (player == null) { throw new NotImplementedException(); }
        return player.TasksToDo;
    }

    public void CompleteTaskByPlayer(int matchId, string playerName, int taskId)
    {
        Models.Match? match = GetMatchById(matchId);
        if (match == null) { throw new NotImplementedException(); }
        Player? player = GetPlayerInMatch(match, playerName);
        if (player == null) { throw new NotImplementedException(); }
        GameTask? task = player.TasksToDo.FirstOrDefault(l => l.Id == taskId);
        if (task != null)
        {
            task.Completed = true;
        }
    }

    public RoleEnum GetRoleByPlayer(int matchId, string playerName)
    {
        Models.Match? match = GetMatchById(matchId);
        Player? player = GetPlayerInMatch(match, playerName);
        if (player != null && player.Role != null)
        {
            return player.Role.RoleTitle;
        }
        throw new NotImplementedException();
    }
}
