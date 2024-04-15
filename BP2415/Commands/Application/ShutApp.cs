using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Enums;

namespace BP2415.Commands.Application;

public abstract class ShutApp : ApplicationCommandsModule
{
    [SlashCommand(name: "shut", isNsfw: true, description: "shuts down the bot")]
    [RequireOwnerOrId(1164458370611298304, 865542945402126356)]
    public static async Task Ping(InteractionContext ctx)
    {
        var bp = ctx.Client;

        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
        {
            Content = "***Shutting down...***"
        });
        await bp.DisconnectAsync();
    }
}