using System.Data;
using Discord;
using Discord.Commands;

public class IdeasModule : ModuleBase<SocketCommandContext> {
    private ulong ideasChannelId = new LoadSecrets().getIdeasChannelId();
    private Emoji tickEmoji = new Emoji("✅");
    private Emoji crossEmoji = new Emoji("❌");

    [Command("idea")]
    public async Task IdeasAsync(params string[] ideaText) {
        string idea = string.Join(" ", ideaText);

        var embed = new EmbedBuilder() {
            Title = "New idea just dropped",
            Description = idea,
            Color = Color.Purple,
            Timestamp = DateTime.Now
        };
        embed.WithAuthor(Context.User);
        
        var channel = Context.Guild.GetChannel(ideasChannelId) as IMessageChannel;
        if (channel == null) {
            return;
        }

        await Context.Message.DeleteAsync();
        var message = await channel.SendMessageAsync(embed: embed.Build());
        await message.AddReactionAsync(tickEmoji);
        await message.AddReactionAsync(crossEmoji);
    }
}