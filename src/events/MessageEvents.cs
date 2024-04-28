using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.VisualBasic;

public class MessageEvents {
    private Emoji hiEmoji = new Emoji("👋");
    private Emoji saluteEmoji = new Emoji("🫡");
    private Random randNum = new Random();
    private readonly ulong _doofRoleId = new LoadSecrets().getDoofRoleId();
    private readonly ulong _modRoleId = new LoadSecrets().getModRoleId();

    public async Task MessageReceived(SocketMessage message) {
        // Guard clause against null and bot messages
        if (message.Author.IsBot || message == null) {
            return;
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

        var user = message.Author as SocketGuildUser;
        bool hasModRole = false;

        foreach (var role in user.Roles)
        {
            if (role.Id.Equals(_modRoleId))
            {
                hasModRole = true;
                break;
            }
        }

        if (hasModRole && (message.Content.ToLower().Contains($"<@&{_doofRoleId}>") || message.Content.ToLower().Contains("@everyone"))) {
            await message.AddReactionAsync(saluteEmoji);
        }
    }
}
