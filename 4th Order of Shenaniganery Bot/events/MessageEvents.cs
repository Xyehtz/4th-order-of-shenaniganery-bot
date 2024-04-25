using Discord.WebSocket;

public class MessageEvents {
    public static async Task MessageReceived(SocketMessage message) {
        // Guard clause against null and bot messages
        if (message.Author.IsBot || message == null) {
            return;
        }

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
