using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Enums;

namespace BP2415.Commands.Slash;

public class PingApp : ApplicationCommandsModule
{
    [SlashCommand(name: "ping", description: "pong")]
    [RequireGuild, RequireOwnerOrId(1164458370611298304, 865542945402126356)]
    public async Task Ping(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
        {
            Content = ":ping_pong: " + ctx.Client.Ping + "ms"
        });
    }
}