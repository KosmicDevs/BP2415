using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;
using DisCatSharp.Enums;

namespace BP2415.Commands.Applications
{
    public class StatusApp : ApplicationCommandsModule
    {
        [SlashCommand("status", "Ändern Sie den Status des Bots")]
        [RequireOwner, Hidden]
        public async Task Status(InteractionContext ctx,
            [Option("status", "Eingabe des neuen Status")] [RemainingText]
            string status)
        {
            await ctx.Client.UpdateStatusAsync(new DiscordActivity($"{status}", ActivityType.Custom));
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder
                {
                    Content = $"Status aktualisiert {status}."
                });
        }
    }
}