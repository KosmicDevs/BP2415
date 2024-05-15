using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Enums;

namespace BP2415.Commands.Application;

public abstract class PingApp : ApplicationCommandsModule
{
    [SlashCommand(name: "ping", description: "pong")]
    public static async Task Ping(InteractionContext ctx)
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