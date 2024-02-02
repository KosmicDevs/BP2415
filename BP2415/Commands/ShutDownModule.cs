using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;

namespace BP2415.Commands;

public class ShutDownModule : BaseCommandModule
{
    [Command(name:"shut")]
    [RequireGuild, RequireOwnerOrId(1164458370611298304, 865542945402126356)]
    public async Task Shut(CommandContext ctx)
    {   
        var bp = ctx.Client;
        
        await ctx.RespondAsync(":red_circle: :red_circle: :red_circle: Shutting down... :red_circle: :red_circle: :red_circle: ");
        await bp.DisconnectAsync();
    }
}