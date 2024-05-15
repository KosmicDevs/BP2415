using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;

namespace BP2415.Commands
{
    public class ShutDownModule : BaseCommandModule
    {
        [Command("shut")]
        [Description("Schaltet den Bot ab")]
        [RequireOwner]
        [Hidden]
        public async Task Shut(CommandContext ctx)
        {
            var bp = ctx.Client;

            await ctx.RespondAsync("***SHerunterfahren...***");
            await bp.DisconnectAsync();
        }
    }
}