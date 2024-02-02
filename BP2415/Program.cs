using System.Reflection;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using BP2415.Commands.Slash;

using DisCatSharp;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.CommandsNext;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.Lavalink;
using DisCatSharp.Net;
using DisCatSharp.ApplicationCommands.EventArgs;

namespace BP2415
{
    internal static class Program
    {
        private static DiscordClient Discord { get; set; } = null!;

        private static void Main(string[] args = null!)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            string json = await File.ReadAllTextAsync("config.json");
            dynamic config = JsonConvert.DeserializeObject(json)!;
            string token = config.discord.token.ToString();

            Discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                MinimumLogLevel = LogLevel.Debug,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContent,
            });
            
            var endpoint = new ConnectionEndpoint
            {
                Hostname = "127.0.0.1",
                Port = 2333
            };

            var lavalinkConfig = new LavalinkConfiguration
            {
                Password = "youshallnotpass",
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint
            };

            var lavalink = Discord.UseLavalink();

            await Discord.ConnectAsync(new DiscordActivity("with the gears.", ActivityType.Watching));
            await lavalink.ConnectAsync(lavalinkConfig);

            var commands = Discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = [config.discord.prefix.ToString()],
            });

            var appCommands = Discord.UseApplicationCommands();

            commands.RegisterCommands(Assembly.GetExecutingAssembly());

            appCommands.SlashCommandExecuted += Slash_SlashCommandExecutedAsync;
            appCommands.SlashCommandErrored += Slash_SlashCommandErroredAsync;

            appCommands.RegisterGuildCommands<PingSlash>(1164458794244395028);
            appCommands.RegisterGuildCommands<ShutSlash>(1164458794244395028);
            appCommands.RegisterGuildCommands<LavaLinkSlash>(1164458794244395028);
            appCommands.RegisterGuildCommands<MusicSlash>(1164458794244395028);
            appCommands.RegisterGuildCommands<JoinSlash>(1164458794244395028);

            await Task.Delay(-1);
        }

        private static Task Slash_SlashCommandExecutedAsync(ApplicationCommandsExtension sender, SlashCommandExecutedEventArgs e)
	    {
		    sender.Client.Logger.LogInformation("Slash: {ContextCommandName}", e.Context.CommandName);
		    return Task.CompletedTask;
	    }

        private static Task Slash_SlashCommandErroredAsync(ApplicationCommandsExtension sender, SlashCommandErrorEventArgs e)
	    {
		    sender.Client.Logger.LogError("Slash: {ExceptionMessage} | CN: {ContextCommandName} | IID: {ContextInteractionId}", e.Exception.Message, e.Context.CommandName, e.Context.InteractionId);
		    return Task.CompletedTask;
	    }
    }
}