using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Enums;
using DisCatSharp.Lavalink;
using DisCatSharp;

namespace BP2415.Commands.Slash
{

	/// <summary>
	/// Commands to connect and disconnect to the voice channel.
	/// </summary>
	public class JoinSlash : ApplicationCommandsModule
	{
		/// <summary>
		/// Connect to the voice channel.
		/// </summary>
		/// <param name="ctx">Interaction context</param>
		[SlashCommand("connect", "Join the voice channel")]
		public static async Task ConnectAsync(InteractionContext ctx)
		{
			var lava = ctx.Client.GetLavalink();

			// Check if the user is currently connected to the voice channel
			if (ctx.Member!.VoiceState == null)
			{
				await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
				{
					IsEphemeral = true,
					Content = "You must be connected to a voice channel to use this command!"
				});
				return;
			}

			// Check if Lavalink connection is established
			if (!lava.ConnectedSessions.Any())
			{
				await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
				{
					IsEphemeral = true,
					Content = "The Lavalink connection is not established!"
				});
				return;
			}

			var node = lava.ConnectedSessions.Values.First();

			// Connect to the channel
			await ctx.Member.VoiceState.Channel!.ConnectAsync(node);

			await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
			{
				Content = $"The bot has joined the channel {ctx.Member.VoiceState.Channel!.Name.InlineCode()}"
			});
		}

		/// <summary>
		/// Disconnect from the voice channel.
		/// </summary>
		/// <param name="ctx">Interaction context</param>
		[SlashCommand("disconnect", "Leave the voice channel")]
		public static async Task LeaveAsync(InteractionContext ctx)
		{
			var lava = ctx.Client.GetLavalink();
			if (!lava.ConnectedSessions.Any())
			{
				await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
				{
					IsEphemeral = true,
					Content = "The Lavalink connection is not established!"
				});
				return;
			}

			var node = lava.ConnectedSessions.Values.First();

			// Get the current Lavalink connection in the guild.
			var connection = node.GetGuildPlayer(ctx.Guild!);

			if (connection == null)
			{
				await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
				{
					IsEphemeral = true,
					Content = "The bot is not connected to the voice channel in this guild!"
				});
				return;
			}

			await connection.DisconnectAsync();

			await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
			{
				Content = "The bot left the voice channel"
			});
		}
	}
}