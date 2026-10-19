using MellemOs.Models;
using MellemOs.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MellemOs.Hubs;

public class MatchHub(MatchService matchService) : Hub
{
    Match? match;
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
        this.match = match;
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
        if (match == null)
        {
            await Clients.Caller.SendAsync("Error", "Match not found");
            return;
        }

        var playerId = playerPositionData.PlayerId;
        var PositionX = playerPositionData.PositionX;
        var PositionY = playerPositionData.PositionY;

        Console.WriteLine($"Broadcasting player position for {playerId} in match {match.Id}: ({PositionX}, {PositionY})");
        await Clients.Group($"match:{match.Id}").SendAsync("PlayerPosition", playerId, PositionX, PositionY);
    }

    private static int GetMatchIdFromRoute(HttpContext? httpContext)
    {
        var routeValue = httpContext?.Request.RouteValues["matchId"]?.ToString();
        if (!int.TryParse(routeValue, out var matchId))
            throw new HubException("invalid_match_id");

        return matchId;
    }


}

public record PlayerPositionData
{
    public string PlayerId { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }
}
