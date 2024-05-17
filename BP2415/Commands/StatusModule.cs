using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;

namespace BP2415.Commands
{
    public class StatusModule : BaseCommandModule
    {
        [Command("status")]
        [Aliases("stats")]
        [Description("Ändern Sie den aktuellen Status des Bots")]
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