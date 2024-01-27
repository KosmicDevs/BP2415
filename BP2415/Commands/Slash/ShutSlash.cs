using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Enums;

namespace BP2415.Commands.Slash;

public class ShutSlash : ApplicationCommandsModule
{
    [SlashCommand(name: "shut", description: "shuts down the bot")]
    public async Task Ping(InteractionContext ctx)
    {
        var bp = ctx.Client;
        
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
        {
            Content = ":red_circle: :red_circle: :red_circle: Shutting down...:red_circle: :red_circle: :red_circle: "
        });
        await bp.DisconnectAsync();
    }
}