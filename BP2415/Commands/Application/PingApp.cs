using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Entities;
using DisCatSharp.Enums;

namespace BP2415.Commands.Application
{
    public class PingApp : ApplicationCommandsModule
    {
        [SlashCommand("ping", "Pingt den Bot an")]
        public async Task Ping(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder
                {
                    Content = ":ping_pong: " + ctx.Client.Ping + "ms"
                });
        }
    }
}