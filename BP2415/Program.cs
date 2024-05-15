using System.Collections;
using BP2415.Commands;
using BP2415.Commands.Applications;
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

        private static async Task MainAsync(IEnumerable args = null!)
        {
            ArgumentNullException.ThrowIfNull(args);
            var json = await File.ReadAllTextAsync("config.json");
            dynamic config = JsonConvert.DeserializeObject(json)!;
            string token = config.discord.token.ToString();
            var guildId = config.discord.guild is long
                ? (ulong)(long)config.discord.guild
                : (ulong)config.discord.guild;

            Discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                MinimumLogLevel = LogLevel.Debug,
                Intents = DiscordIntents.All,
                Locale = "de_DE",
                Timezone = TimeZoneInfo.Utc.ToString(),
                MobileStatus = true
            });

            await Discord.ConnectAsync(new DiscordActivity("AAAA", ActivityType.Custom));

            var commands = Discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = [config.discord.prefix.ToString()],
            });

            var appCommands = Discord.UseApplicationCommands();

            commands.RegisterCommands<StatusModule>();
            commands.RegisterCommands<PingModule>();
            commands.RegisterCommands<ShutDownModule>();

            appCommands.SlashCommandExecuted += Slash_SlashCommandExecutedAsync;
            appCommands.SlashCommandErrored += Slash_SlashCommandErroredAsync;

            appCommands.RegisterGuildCommands<PingApp>(guildId);
            appCommands.RegisterGuildCommands<ShutApp>(guildId);
            appCommands.RegisterGuildCommands<StatusApp>(guildId);

            await Task.Delay(-1);
        }

        private static Task Slash_SlashCommandExecutedAsync(ApplicationCommandsExtension sender,
            SlashCommandExecutedEventArgs e)
        {
            sender.Client.Logger.LogInformation("Applications: {ContextCommandName}", e.Context.CommandName);
            return Task.CompletedTask;
        }

        private static Task Slash_SlashCommandErroredAsync(ApplicationCommandsExtension sender,
            SlashCommandErrorEventArgs e)
        {
            sender.Client.Logger.LogError(
                "Applications: {ExceptionMessage} | CN: {ContextCommandName} | IID: {ContextInteractionId}",
                e.Exception.Message, e.Context.CommandName, e.Context.InteractionId);
            return Task.CompletedTask;
        }
    }
}