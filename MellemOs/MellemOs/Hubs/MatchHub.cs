using MellemOs.Models;
using MellemOs.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MellemOs.Hubs;

public class MatchHub(MatchService matchService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var matchId = GetMatchIdFromRoute(Context.GetHttpContext());
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

        var playerId = playerPositionData.PlayerId;
        var positionX = playerPositionData.PositionX;
        var positionY = playerPositionData.PositionY;

        Console.WriteLine($"Broadcasting player position for {playerId} in match {matchId}: ({positionX}, {positionY})");
        await Clients.Group($"match:{matchId}").SendAsync("PlayerPosition", playerId, positionX, positionY);
    }

    private static int GetMatchIdFromRoute(HttpContext? httpContext)
    {
        var routeValue = httpContext?.Request.RouteValues["matchId"]?.ToString();
        if (!int.TryParse(routeValue, out var matchId))
            throw new HubException("invalid_match_id");

        return matchId;
    }

    public async Task<Player[]> GetPlayers()
    {
        if (Context.Items["matchId"] is not int matchId)
        {
            throw new HubException("Match ID not found in context items");
        }

        Match match = matchService.GetMatchById(matchId) ?? throw new HubException("Match not found");
        return match.Players;

    }


}

public record PlayerPositionData
{
    public string PlayerId { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }
}
