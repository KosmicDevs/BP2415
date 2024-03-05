using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Converters;
using DisCatSharp.Entities;

namespace BP2415.Commands;

public class HelpModule : DefaultHelpFormatter
{
    public HelpModule(CommandContext ctx) : base(ctx) {  }

    public async Task HelpAsync(CommandContext ctx)
    {
        var commands = ctx.CommandsNext.RegisteredCommands.Values.Where(x => x.Module.ToString() == ctx.Command.Module.ToString()).ToList();

        var embed = new DiscordEmbedBuilder()
            .WithTitle("Help")
            .WithColor(new DiscordColor(16777215))
            .WithDescription("Here are all the commands I have for you!");

        foreach (var command in commands)
        {
            embed.AddField(new DiscordEmbedField(command.Name, command.Description));

            if (command.Aliases.Count > 0)
            {
                embed.AddField(new DiscordEmbedField("Aliases", string.Join(", ", command.Aliases)));
                embed.AddField(new DiscordEmbedField("Description", command.Description));
                embed.AddField(new DiscordEmbedField("Is Hidden", command.IsHidden.ToString()));
            }
        }
        await ctx.RespondAsync(embed: embed.Build());
    }
}