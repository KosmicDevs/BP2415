using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;

namespace BP2415.Commands
{
    public class ShutDownModule : BaseCommandModule
    {
        [Command("shut")]
        [RequireOwnerOrId(1164458370611298304, 865542945402126356)]
        public async Task Shut(CommandContext ctx)
        {
            var bp = ctx.Client;

            await ctx.RespondAsync("***SHerunterfahren...***");
            await bp.DisconnectAsync();
        }
    }
}