using DisCatSharp.Entities;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.Enums;
using DisCatSharp.Lavalink;
using DisCatSharp.Lavalink.Entities;
using DisCatSharp.Lavalink.Enums;
using DisCatSharp.ApplicationCommands;
using DisCatSharp;

namespace BP2415.Commands.Slash
{
    public class LavaLinkSlash : ApplicationCommandsModule
    {
        [SlashCommand("join", "Join a voice channel")]
        public async Task JoinAsync(InteractionContext ctx, [Option("channel", "Channel to join")] DiscordChannel channel)
        {
            var lavalink = ctx.Client.GetLavalink();
            var session = lavalink.ConnectedSessions.Values.First();

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            
            if (!lavalink.ConnectedSessions.Any())
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new() {
                    IsEphemeral = true,
                    Content = "The Lavalink connection is not established!"
                });
                return;
            }

            if (ctx.Member!.VoiceState == null || ctx.Member.VoiceState.Channel! == null!)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new() {
                    IsEphemeral = true,
                    Content = "You must be in a voice channel to use this command"
                });
                return;
            }
            await channel.ConnectAsync(session);
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
		    {
			    Content = $"The bot has joined the channel {channel.Mention()}"
		    });
        }

        [SlashCommand("leave", "Leave the voice channel")]
        public async Task LeaveAsync(InteractionContext ctx)
        {
            var lavalink = ctx.Client.GetLavalink();
            var session = lavalink.ConnectedSessions.Values.First();
            var guildPlayer = session.GetGuildPlayer(ctx.Member!.VoiceState!.Guild!);

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            if (!lavalink.ConnectedSessions.Any())
			{
				await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
				{
					IsEphemeral = true,
					Content = "The Lavalink connection is not established!"
				});
				return;
			}

            if (guildPlayer == null)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new() {
                    IsEphemeral = true,
                    Content = "The bot is not connected to the voice channel in this guild!"
                });
                return;
            }
            await guildPlayer.DisconnectAsync();
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new() {
                    IsEphemeral = true,
                    Content = $"Left {guildPlayer.Channel.Mention}"
            });
        }

        [SlashCommand("play", "Play a track")]
        public async Task PlayAsync(InteractionContext ctx, [Option("query", "The query to search for")] string query)
        {
            var lavalink = ctx.Client.GetLavalink();
            var session = lavalink.ConnectedSessions.Values.First();
            // var guildPlayer = session.GetGuildPlayer(ctx.Member!.VoiceState!.Guild!);
            // var loadResult = await guildPlayer!.LoadTracksAsync(LavalinkSearchType.Youtube, query);

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            // if (ctx.Member!.VoiceState == null || ctx.Member.VoiceState.Channel! == null!)
            // {
            //     await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("You are not in a voice channel."));
            //     return;
            // }
        
            // if (guildPlayer == null)
            // {
            //     await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Lavalink is not connected."));
            //     return;
            // }
            
            // if (loadResult.LoadType == LavalinkLoadResultType.Empty ||
            //     loadResult.LoadType == LavalinkLoadResultType.Error)
            // {
            //     await ctx.EditResponseAsync(
            //         new DiscordWebhookBuilder().WithContent($"Track search failed for {query}."));
            //     return;
            // }

            // LavalinkTrack track = loadResult.LoadType switch
            // {
            //     LavalinkLoadResultType.Track => loadResult.GetResultAs<LavalinkTrack>(),
            //     LavalinkLoadResultType.Playlist => loadResult.GetResultAs<LavalinkPlaylist>().Tracks.First(),
            //     LavalinkLoadResultType.Search => loadResult.GetResultAs<List<LavalinkTrack>>().First(),
            //     _ => throw new InvalidOperationException("Unexpected load result type.")
            // };
            // await guildPlayer.PlayAsync(track);
            // await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Now playing {query}!"));

            var connection = session.GetGuildPlayer(ctx.Member!.VoiceState!.Guild!);

            if (connection == null)
			{
				await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
				{
					IsEphemeral = true,
					Content = "The bot is not connected to the voice channel in this guild!"
				});
				return;
			}

			if (ctx.Member.VoiceState == null || ctx.Member!.VoiceState!.Channel! == null! ||
			    ctx.Member.VoiceState.Channel != connection.Channel)
			{
				await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
				{
					IsEphemeral = true,
					Content = "You must be in the same voice channel as the bot!"
				});
				return;
			}

            LavalinkTrackLoadingResult tracks;

            // Check if query is valid url
		    if (Uri.TryCreate(query, UriKind.Absolute, out var uriResult) &&
		        (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
			// Get track from the url
			    tracks = await connection.LoadTracksAsync(uriResult.AbsolutePath);
		    else
			// Search track in Youtube
			    tracks = await connection.LoadTracksAsync(query);
		    // If something went wrong on Lavalink's end or it just couldn't find anything.
		    if (tracks.LoadType is LavalinkLoadResultType.Error or LavalinkLoadResultType.Empty)
		    {
			    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
			    {
				    IsEphemeral = true,
				    Content = $"Track search failed for `{query}`."
			});
			return;
		    }
            // Get first track in the result
		    var track = tracks.GetResultAs<LavalinkPlaylist>().Tracks.First();

		    await connection.PlayAsync(track);
		    // CHALLENGE: Add a queue. You need to make sure that new tracks are added to a
            // special queue instead of overwriting the current one
		    // and automatically played after the end of the previous track.
		    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
		    {
			    Content = $"Now playing {track.Info.Author.InlineCode()} - {track.Info.Title.InlineCode()}"
		    });
        }

        [SlashCommand("pause", "Pause a track")]
        public async Task PauseAsync(InteractionContext ctx)
        {
            var lavalink = ctx.Client.GetLavalink();
            var guildPlayer = lavalink.GetGuildPlayer(ctx.Guild!);

            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            
            if (ctx.Member!.VoiceState == null || ctx.Member.VoiceState.Channel! == null!)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("You are not in a voice channel."));
                return;
            }

            if (guildPlayer == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Lavalink is not connected."));
                return;
            }

            if (guildPlayer.CurrentTrack == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("There are no tracks loaded."));
                return;
            }
            await guildPlayer.PauseAsync();
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Playback paused!"));
        }

        [SlashCommand("resume", "Resume a track")]
        public async Task ResumeAsync(InteractionContext ctx)
        {
            var lavalink = ctx.Client.GetLavalink();
            var guildPlayer = lavalink.GetGuildPlayer(ctx.Guild!);
            
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            
            if (ctx.Member!.VoiceState == null || ctx.Member.VoiceState.Channel! == null!)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("You are not in a voice channel."));
                return;
            }

            if (guildPlayer == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Lavalink is not connected."));
                return;
            }

            if (guildPlayer.CurrentTrack == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("There are no tracks loaded."));
                return;
            }
            await guildPlayer.ResumeAsync();
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Playback resumed!"));
        }

        [SlashCommand("testlavalink", "Tests if Lavalink is connected")]
        public async Task TestLavalink(InteractionContext ctx)
        {
            var lavalink = ctx.Client.GetLavalink();

            if (lavalink.ConnectedSessions.Count == 0)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("Lavalink is not connected!"));
                return;
            }

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent("Lavalink is connected!"));
        }
    }
}