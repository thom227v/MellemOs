using MellemOs.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Numerics;
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
    private int _matchIdCounter;
    private List<Models.Match> Matches { get; set; } = new();


    /// <summary>
    /// Registers a vote for a player and checks whether the voting round is complete.
    /// </summary>
    /// <param name="matchId">The ID of the match in which the vote is being cast.</param>
    /// <param name="playername">The name of the player casting the vote.</param>
    /// <param name="voteeName">The name of the player receiving the vote.</param>
    /// <returns>A tuple indicating whether the voting-round is complete and the name of the player voted out, if any.</returns>
    public (bool, string?) RegisterVote(int matchId, string playername, string voteeName)
    {
        Models.Match? match = GetMatchById(matchId);
        if (match == null) throw new NotImplementedException();
        if (!match.Votes.TryAdd(voteeName, 1)) match.Votes[voteeName]++;

        // Check if everyone has voted
        int totalVotes = match.Votes.Values.Sum();
        if (totalVotes == match.Players.Length)
        {
            // Determine the player with the most votes
            var maxVotes = match.Votes.Values.Max();
            var votedOutPlayers = match.Votes.Where(v => v.Value == maxVotes).Select(v => v.Key).ToList();

            if (votedOutPlayers.Count == 1)
            {
                string votedOutPlayerName = votedOutPlayers[0];
                Player? votedOutPlayer = match.Players.FirstOrDefault(p => p.Name == votedOutPlayerName);
                if (votedOutPlayer != null)
                {
                    votedOutPlayer.IsAlive = false;
                    return (true, votedOutPlayerName);
                }
            }
            else
            {
                // Handle tie situation
                Console.WriteLine($"Voting resulted in a tie between players: {string.Join(", ", votedOutPlayers)}. No player will be voted out.");
                return (true, "Skip");
            }

            // Reset votes for the next round
            match.Votes.Clear();
        }
        return (false, null);
    }


    private void AssignRoles(ref Models.Match match)
    {
        Random rnd = new Random();

        Player[] tempPlayers = (Player[])match.Players.Clone();
        Player murdererPlayer = match.Players[rnd.Next(0, tempPlayers.Count()-1)];
        //TempPlayers.ToList().Remove(murdererPlayer);
        //Player detectivePlayer = match.Players[rnd.Next(0, TempPlayers.Count()-1)];

        foreach (Player player in match.Players)
        {
            if (murdererPlayer.Name == player.Name)
            {
                player.Role = new Role { RoleTitle = RoleEnum.Murderer, AbilityCooldown = 15 };
                continue;
            }
            //if (detectivePlayer.Name == player.Name)
            //{
            //    player.Role = new Role { RoleTitle = RoleEnum.Detective, AbilityCooldown = 15 };
            //    continue;
            //}

            player.TasksToDo = _taskService.GenerateRndTasks();
            player.Role = new Role { RoleTitle = RoleEnum.Innocent, AbilityCooldown = 15 };
        }
    }

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

    public (int, int) GetTasksProgress(int matchId)
    {
        Models.Match? match = GetMatchById(matchId);
        if (match == null) { throw new NotImplementedException(); }
        int totalTasks = match.Players.Sum(x => x.TasksToDo.Count);
        int totalTasksCompleted = match.Players.Sum(p => p.TasksToDo.Count(t => t.Completed));
        return (totalTasksCompleted, totalTasks);
    }

    public Role GetRoleByPlayer(int matchId, string playerName)
    {
        Models.Match? match = GetMatchById(matchId);
        Player? player = GetPlayerInMatch(match, playerName);
        if (player != null && player.Role != null)
        {
            return player.Role;
        }
        throw new NotImplementedException();
    }

    public Player[] KillPlayer(int matchId, string playername)
    {
        Models.Match? match = GetMatchById(matchId);
        if (match == null) { throw new NotImplementedException(); }
        Player? player = GetPlayerInMatch(match, playername);
        if (player == null) { throw new NotImplementedException(); }
        player.IsAlive = false;
        return match.Players;
    }
}
