using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Enums;

namespace BP2415.Commands.Slash;

public class ShutApp : ApplicationCommandsModule
{
    [SlashCommand(name: "shut", isNsfw: true, description: "shuts down the bot")]
    [RequireOwnerOrId(1164458370611298304, 865542945402126356)]
    public async Task Ping(InteractionContext ctx)
    {
        var bp = ctx.Client;
        
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
        {
            Content = ":red_circle: :red_circle: :red_circle: Shutting down... :red_circle: :red_circle: :red_circle: "
        });
        await bp.DisconnectAsync();
    }
}