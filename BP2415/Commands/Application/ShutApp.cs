using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;
using DisCatSharp.Enums;

namespace BP2415.Commands.Application
{
    public class ShutApp : ApplicationCommandsModule
    {
        [SlashCommand("shut", "Schaltet den Bot ab")]
        [RequireOwner]
        [Hidden]
        public async Task Ping(InteractionContext ctx)
        {
            var bp = ctx.Client;

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder
                {
                    Content = "***Herunterfahren...***"
                });
            await bp.DisconnectAsync();
        }
    }
}