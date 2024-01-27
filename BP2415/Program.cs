using System.Reflection;

using BP2415.Commands.Slash;

using Newtonsoft.Json;

using DisCatSharp;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.CommandsNext;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.Interactivity.Extensions;

namespace BP2415
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Sailors!");
            MainAsync().GetAwaiter().GetResult();
        }
        
        static async Task MainAsync()
        {
            string json = await File.ReadAllTextAsync("config.json");
            dynamic config = JsonConvert.DeserializeObject(json)!;
            string token = config.discord.token.ToString();
            
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
            });

            var status = discord.UpdateStatusAsync(new DiscordActivity("waiting for commands", ActivityType.Watching), UserStatus.Idle);
            
            var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new List<string>() { config.discord.prefix.ToString() },
            });
            
            var appCommands = discord.UseApplicationCommands();

            commands.RegisterCommands(Assembly.GetExecutingAssembly());
            
            appCommands.RegisterGuildCommands<PingSlash>(1164458794244395028);
            appCommands.RegisterGuildCommands<ShutSlash>(1164458794244395028);
            
            // await appCommands.CleanGuildCommandsAsync();
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}