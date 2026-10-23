using MellemOs.Models;
using MellemOs.Services;
using Microsoft.AspNetCore.SignalR;

namespace MellemOs.Hubs;

public class MatchHub(MatchService matchService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var matchId = GetMatchIdFromRoute(Context.GetHttpContext());
        Console.WriteLine($"Client {Context.ConnectionId} is connecting to match {matchId}");

        Match? match =  matchService.GetMatchById(matchId);
        if (match == null)
        {
            await Clients.Caller.SendAsync("Error", "Match not found");
            Context.Abort();
            return;
        }

        Context.Items["matchId"] = matchId;

        await Groups.AddToGroupAsync(Context.ConnectionId, $"match:{matchId}");
        Console.WriteLine($"Client {Context.ConnectionId} connected to match {matchId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task BroadCastPlayerPosition(PlayerPositionData playerPositionData)
    {
        Console.WriteLine("hello");
        if (Context.Items["matchId"] is not int matchId)
        {
            Console.WriteLine("Match ID not found in context items");
            await Clients.Caller.SendAsync("Error", "Match ID not found");
            return;
        }

        Match? match = matchService.GetMatchById(matchId);
        if (match != null)
        {
            Player? player = matchService.GetPlayerInMatch(match, playerPositionData.PlayerId);
            if (player != null && player.IsAlive)
            {
                var playerId = playerPositionData.PlayerId;
                var positionX = playerPositionData.PositionX;
                var positionY = playerPositionData.PositionY;

                Console.WriteLine($"Broadcasting player position for {playerId} in match {matchId}: ({positionX}, {positionY})");
                await Clients.Group($"match:{matchId}").SendAsync("PlayerPosition", playerPositionData);
            }
        }
    }

    private static int GetMatchIdFromRoute(HttpContext? httpContext)
    {
        var routeValue = httpContext?.Request.RouteValues["matchId"]?.ToString();
        if (!int.TryParse(routeValue, out var matchId))
            throw new HubException("invalid_match_id");

        return matchId;
    }

    public async Task<List<GameTask>> GetTasksByPlayerName(string playername)
    {
        return matchService.GetTasksByPlayer(GetMatchIdFromRoute(Context.GetHttpContext()), playername);
    }

    public async Task<Role> GetRoleByPlayerName(string playername)
    {
        return matchService.GetRoleByPlayer(GetMatchIdFromRoute(Context.GetHttpContext()), playername);
    }

    public async Task<Player[]> KillPlayer(string playerName)
    {
        var matchId = GetMatchIdFromRoute(Context.GetHttpContext());
        await Clients.Group($"match:{matchId}").SendAsync("PlayerDied", playerName);

        Player[] players = matchService.KillPlayer(matchId, playerName);

        bool? gameFinished = matchService.GameFinished(matchId);
        if (gameFinished != null)
        {
            await Clients.Group($"match:{matchId}").SendAsync("GameFinished", gameFinished);
        }

        return players;
    }

    public async Task<Player[]?> GetPlayers()
    {
        if (Context.Items["matchId"] is not int matchId)
        {
            Console.Write("Match ID not found in context items");
            return null;
        }
        Console.WriteLine("Trying to get match by id: " + matchId);
        Match? match = matchService.GetMatchById(matchId);
        if (match == null)
        {
            Console.WriteLine("actual id: " + matchService.GetMatchById(1));
            Console.WriteLine("ERROR: match not found");
        }
        else
        {
            return match.Players;
        }
        return null;
    }
    public async void CompleteTaskByPlayerName(string playername, int taskId)
    {
        var matchId = GetMatchIdFromRoute(Context.GetHttpContext());
        Console.WriteLine("matchId" + matchId);
        matchService.CompleteTaskByPlayer(matchId, playername, taskId);

        (int, int) taskProgress = matchService.GetTasksProgress(matchId);
        await Clients.Group($"match:{matchId}").SendAsync("TotalTasks", taskProgress.Item1, taskProgress.Item2);

        bool? gameFinished = matchService.GameFinished(matchId);
        if (gameFinished != null)
        {
            await Clients.Group($"match:{matchId}").SendAsync("GameFinished", gameFinished);
        }
    }

    public void StartVote(string username)
    {
        var matchId = GetMatchIdFromRoute(Context.GetHttpContext());
        var match = matchService.GetMatchById(matchId);
        if (match == null)
        {
            throw new NotImplementedException();
        }

        Player? player = matchService.GetPlayerInMatch(match, username);
        if(player == null || player.IsAlive == false)
        {
            return;
        }

        match.Votes.Clear();

        Clients.Group($"match:{matchId}").SendAsync("VotingStarted");
    }

    public void RegisterVote(string playername, string voteeName)
    {
        var matchId = GetMatchIdFromRoute(Context.GetHttpContext());

        Match? match = matchService.GetMatchById(matchId);
        if (match == null) { return ; }
        Player? player = matchService.GetPlayerInMatch(match, playername);
        if (player == null ||player.IsAlive == false) { return; }

        var (votingCompleted, votedPlayer) = matchService.RegisterVote(matchId, playername, voteeName);

        if (votingCompleted)
        {
            Console.WriteLine($"Voting completed in match {matchId}. Player {votedPlayer} has been voted out.");
            Clients.Group($"match:{matchId}").SendAsync("VotingCompleted", votedPlayer);
            //if (votedPlayer != "Skip")
            //{
            //    Clients.Group($"match:{matchId}").SendAsync("PlayerDied", votedPlayer);
            //}
        }
        else
        {
            Console.WriteLine($"Vote registered: {playername} voted for {voteeName} in match {matchId}");
            Clients.Group($"match:{matchId}").SendAsync("VoteRegistered", playername, voteeName);
        }

       // Clients.Group($"match:{matchId}").SendAsync("PlayerDied", votedPlayer);
    }
}

public record PlayerPositionData
{
    public string PlayerId { get; set; } = String.Empty;
    public float PositionX { get; set; }
    public float PositionY { get; set; }
}
