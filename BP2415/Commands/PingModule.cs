using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;

namespace BP2415.Commands;

public class PingModule : BaseCommandModule
{
    [Command("ping")]
    [Aliases("pong")]
    [Description("Pingt den Bot an")]
    public async Task Ping(CommandContext ctx)
    {
        await ctx.RespondAsync(":ping_pong: " + ctx.Client.Ping + "ms");
    }
}