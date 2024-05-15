using BP2415.Commands;
using BP2415.Commands.Application;
using DisCatSharp;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.EventArgs;
using DisCatSharp.CommandsNext;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            var json = await File.ReadAllTextAsync("config.json");
            dynamic config = JsonConvert.DeserializeObject(json)!;
            string token = config.discord.token.ToString();
            ulong guildId = config.discord.guild.ToString();

            Discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                MinimumLogLevel = LogLevel.Debug,
                Locale = "de_DE",
                Timezone = TimeZoneInfo.Utc.ToString(),
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContent,
            });

            await Discord.ConnectAsync(new DiscordActivity("with the gears bp!help", ActivityType.Watching));

            var commands = Discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = [config.discord.prefix.ToString()],
            });

            commands.RegisterCommands<PingModule>();
            commands.RegisterCommands<ShutDownModule>();
            commands.RegisterCommands<StatusModule>();

            var appCommands = Discord.UseApplicationCommands();

            appCommands.SlashCommandExecuted += Slash_SlashCommandExecutedAsync;
            appCommands.SlashCommandErrored += Slash_SlashCommandErroredAsync;

            appCommands.RegisterGuildCommands<PingApp>(guildId);
            appCommands.RegisterGuildCommands<ShutApp>(guildId);

            await Task.Delay(-1);
        }

        private static Task Slash_SlashCommandExecutedAsync(ApplicationCommandsExtension sender,
            SlashCommandExecutedEventArgs e)
        {
            sender.Client.Logger.LogInformation("Application: {ContextCommandName}", e.Context.CommandName);
            return Task.CompletedTask;
        }

        private static Task Slash_SlashCommandErroredAsync(ApplicationCommandsExtension sender,
            SlashCommandErrorEventArgs e)
        {
            sender.Client.Logger.LogError(
                "Application: {ExceptionMessage} | CN: {ContextCommandName} | IID: {ContextInteractionId}",
                e.Exception.Message, e.Context.CommandName, e.Context.InteractionId);
            return Task.CompletedTask;
        }
    }
}