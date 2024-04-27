using Discord;
using Discord.WebSocket;

public class MessageEvents {
    private static ulong _ideasChannelId;

    public MessageEvents(ulong ideasChannelId) {
        _ideasChannelId = ideasChannelId;
    }

    public async Task MessageReceived(SocketMessage message) {
        // Guard clause against null and bot messages
        if (message.Author.IsBot || message == null) {
            return;
        } 

            // var embed = new EmbedBuilder() {
            //     Title = "New idea just dropped",
            //     Description = message.Content.Substring(10, message.Content.Length - 10),
            //     Color = Color.Purple,
            //     Timestamp = DateTime.Now,
            //     Fields = new List<EmbedFieldBuilder> {
            //         new EmbedFieldBuilder() {
            //             Name = "Author",
            //             Value = message.Author.Mention,
            //             IsInline = true
            //         }
            //     }
            // };

            // await message.Channel.SendMessageAsync(embed: embed.Build());
            // await message.DeleteAsync();

        // Case switch to respond to different messages
        switch (message.Content.ToLower()) {
            case "!hello":
                await message.Channel.SendMessageAsync($"Hello {message.Author.Mention}");
                break;

            case "!caseTwo":
                await message.Channel.SendMessageAsync("What?");
                break;
        }
    }
}
