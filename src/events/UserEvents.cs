using Discord.WebSocket;
using Discord;

public class UserEvents {
    private readonly ulong _welcomeChannelId = new LoadSecrets().getWelcomeChannelId();
    private readonly ulong _modChannelId = new LoadSecrets().getModChannelId();

    // Event for when a user joins the server
    public async Task UserJoined(SocketGuildUser user) {

        // Write flat content to a file to contain the logs of users joining the server
        using (StreamWriter outputFile = File.AppendText("logs/join_user.txt"))
        {
            await outputFile.WriteAsync($"{user.Username};{user.DisplayName};{user.JoinedAt}\n");
        }

        // Get the channel and check if its not null using the guard clause
        var welcomeChannel = user.Guild.GetChannel(_welcomeChannelId) as IMessageChannel;
        if (welcomeChannel == null) {
            Console.WriteLine("Error sending message on the channel");
            return;
        }

        // Check if the user has been banned and if so, send a message to the mod channel
        string bannedUsers = File.ReadAllText("logs/banned_user.txt");
        if (bannedUsers.Contains(user.Username)) {

            // Set the times banned to zero, split the content of the file line by line
            int timesBanned = 0;
            string[] usernames = bannedUsers.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            // Count and increase the counter depending on how many times the user has been banned
            foreach (string username in usernames) {
                timesBanned++;
            }

            // Check if the channel exists
            var modChannel = user.Guild.GetChannel(_modChannelId) as IMessageChannel;
            if (modChannel == null) {
                Console.WriteLine("Error sending message on the channel");
                return;
            }

            // Send a message to the mod channel
            await modChannel.SendMessageAsync($"{user.Username} ({user.DisplayName}) has joined the server. The user was previously banned. The user has been banned {timesBanned} time(s)");
        }

        // Send a message to the welcome channel and DM the user
        await welcomeChannel.SendMessageAsync($"Hey {user.Mention} welcome to ***4th Order of Shenaniganery***");
    }

    // Event for when a user leaves the server
    public async Task UserLeft(SocketGuild guild, SocketUser user) {
        // Write flat content to a file to contain the logs of users leaving the server
        using (StreamWriter outputFile = File.AppendText("logs/leave_user.txt")) {
            await outputFile.WriteAsync($"{user.Username};{user.GlobalName}\n");
        }
    }

    public async Task UserBanned(SocketUser user, SocketGuild guild) {

        // Write flat content to a file containing user, time and reason
        using (StreamWriter outputFile = File.AppendText("logs/banned_user.txt"))
        {
            await outputFile.WriteAsync($"{user.Username}\n");
        }

        // Get the channel and check if its not null using the guard clause
        var modChannel = guild.GetChannel(_modChannelId) as IMessageChannel;
        if (modChannel == null) {
            Console.WriteLine("Error sending message on the channel");
            return;
        }

        await modChannel.SendMessageAsync($"{user.Username} ({user.GlobalName}) has been banned");
    }

    public async Task UserUnbanned(SocketUser user, SocketGuild guild) {

        var modChannel = guild.GetChannel(_modChannelId) as IMessageChannel;
        if (modChannel == null) {
            Console.WriteLine("Error sending message on the channel");
            return;
        }

        await modChannel.SendMessageAsync($"{user.Username} ({user.GlobalName}) has been unbanned. The ban record will not be removed");
    }
}
