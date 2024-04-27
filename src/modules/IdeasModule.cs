using System.Data;
using Discord;
using Discord.Commands;

[NamedArgumentType]
public class IdeasModuleArgs {
    public string idea { get; set; }
}

public class IdeasModule : ModuleBase<SocketCommandContext> {
    private ulong ideasChannelId = new LoadSecrets().getIdeasChannelId();
    [Command("idea")]
    public async Task IdeasAsync(params string[] ideaText) {
        string idea = string.Join(" ", ideaText);

        var embed = new EmbedBuilder()
        {
            Title = "New idea just dropped",
            Description = idea,
            Color = Color.Purple,
            Timestamp = DateTime.Now
        };

        embed.WithAuthor(Context.User);

        await Context.Message.DeleteAsync();
        
        var channel = Context.Guild.GetChannel(ideasChannelId) as IMessageChannel;

        if (channel == null) {
            return;
        }

        await channel.SendMessageAsync(embed: embed.Build());
    }
}