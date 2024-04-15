using DisCatSharp;
using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace BP2415.Commands
{
    public class StatusModule : BaseCommandModule
    {
        internal static DiscordClient Discord { get; set; } = null!;

        [Command("status")]
        [Aliases("stats")]
        [RequireOwner]
        [Hidden]
        public async Task Status(CommandContext ctx, [RemainingText] string status)
        {
            await ctx.Client.UpdateStatusAsync(new DiscordActivity($"{status}", ActivityType.Custom));
            await ctx.RespondAsync($"Status aktualisiert {status}");

            if (!ctx.Client.UpdateStatusAsync(new DiscordActivity($"{status}", ActivityType.Custom)).IsCompleted)
                Console.WriteLine("Status kann nicht aktualisiert werden.()");
        }
    }
}