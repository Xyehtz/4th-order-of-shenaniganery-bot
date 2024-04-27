using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.VisualBasic;

public class MessageEvents {
    private static ulong _ideasChannelId;
    private Emoji hiEmoji = new Emoji("👋");
    private Random randNum = new Random();

    public MessageEvents(ulong ideasChannelId) {
        _ideasChannelId = ideasChannelId;
    }

    public async Task MessageReceived(SocketMessage message) {
        // Guard clause against null and bot messages
        if (message.Author.IsBot || message == null) {
            return;
        }

        if (message.Channel.Id == _ideasChannelId) {
            var embed = new EmbedBuilder() {
                Title = "New idea just dropped",
                Description = message.Content.Substring(10, message.Content.Length - 10),
                Color = Color.Purple,
                Timestamp = DateTime.Now,
                Fields = new List<EmbedFieldBuilder> {
                    new EmbedFieldBuilder() {
                        Name = "Author",
                        Value = message.Author.Mention,
                        IsInline = true
                    }
                }
            };

            await message.Channel.SendMessageAsync(embed: embed.Build());
            await message.DeleteAsync();
        }

        // Case switch to respond to different messages
        switch (message.Content.ToLower()) {
            case "hello":
                await message.Channel.SendMessageAsync($"Hello {message.Author.Mention}");
                await message.AddReactionAsync(hiEmoji);
                break;

            case "hi":
                await message.Channel.SendMessageAsync($"Hello {message.Author.Mention}");
                await message.AddReactionAsync(hiEmoji);
                break;

            case "hey":
                await message.Channel.SendMessageAsync($"Hello {message.Author.Mention}");
                await message.AddReactionAsync(hiEmoji);
                break;

            case "random number":
                await message.Channel.SendMessageAsync($"Your random number is: {randNum.Next(0, 1000)}");
                break;

            case "rand num":
                await message.Channel.SendMessageAsync($"Your random number is: {randNum.Next(0, 1000)}");
                break;

            case "rand":
                await message.Channel.SendMessageAsync($"Your random number is: {randNum.Next(0, 1000)}");
                break;
        }
    }
}
