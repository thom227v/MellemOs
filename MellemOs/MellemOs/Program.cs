using MellemOs.Hubs;
using MellemOs.Models;
using MellemOs.Services;

namespace MellemOs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<Lobbies>();
            builder.Services.AddSingleton<LobbyService>();
            builder.Services.AddSingleton<MatchService>();
            builder.Services.AddSingleton<TaskService>();
            var app = builder.Build();

            app.MapHub<LobbyHub>("/hubs/lobby");
            app.MapHub<MatchHub>("/hubs/match/{matchId}");


            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
