using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.Lavalink;
using DisCatSharp.Lavalink.Enums;
using DisCatSharp.ApplicationCommands;

namespace BP2415.Commands.Slash
{
    public class LavaSlash : ApplicationCommandsModule
    {
        [SlashCommand("play", "Play a track")]
        public async Task PlayAsync(InteractionContext ctx, [Option("query", "The query to search for")] string query)
        {
            var lavalink = ctx.Client.GetLavalink();
            // loadresult


            if (loadResult.LoadResultType == LavalinkLoadResultType.Empty ||
                loadResult.LoadResultType == LavalinkLoadResultType.Error)
            {
                await ctx.FollowUpAsync($"Track search failed for `{query}`.");
                return;
            }

            var track = loadResult.Tracks.FirstOrDefault();

            if (track == null)
            {
                await ctx.FollowUpAsync("No tracks found.");
                return;
            }
            // Rest of method
        }
    }
}