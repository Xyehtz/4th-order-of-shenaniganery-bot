using Discord.WebSocket;
using Discord;

public class UserEvents {

    // Event for when a user joins the server
    public static async Task UserJoined(SocketGuildUser user) {

        // Load .env and get the welcome channel id and bot channel id
        DotNetEnv.Env.Load(@"../.env");
        ulong channelId = Convert.ToUInt64(DotNetEnv.Env.GetString("WELCOME_CHANNEL_ID"));
        ulong modChannelId = Convert.ToUInt64(DotNetEnv.Env.GetString("TEST_CHANNEL_ID"));

        // Write flat content to a file to contain the logs of users joining the server
        using (StreamWriter outputFile = File.AppendText("logs/join_user.txt"))
        {
            await outputFile.WriteAsync($"{user.Username};{user.DisplayName};{user.JoinedAt}\n");
        }

        // Get the channel and check if its not null using the guard clause
        var channel = user.Guild.GetChannel(channelId) as IMessageChannel;
        if (channel == null) {
            Console.WriteLine("Error sending message on the channel");
            return;
        }

        // Check if the user has been banned and if so, send a message to the mod channel
        if (File.ReadAllText("logs/banned_user.txt").Contains(user.Username)) {
            var botChannel = user.Guild.GetChannel(modChannelId) as IMessageChannel;

            if (botChannel == null) {
                Console.WriteLine("Error sending message on the channel");
                return;
            }

            await botChannel.SendMessageAsync($"{user.Username} ({user.DisplayName}) has joined the server. The user was previously banned");
        }

        // Send a message to the welcome channel and DM the user
        await channel.SendMessageAsync($"Hey {user.Mention} welcome to ***4th Order of Shenaniganery***");
        await user.SendMessageAsync("Hey! Welcome to *** 4th Of Shenaniganery ***");
    }

    // Event for when a user leaves the server
    public static async Task UserLeft(SocketGuild guild, SocketUser user) {
        // Write flat content to a file to contain the logs of users leaving the server
        using (StreamWriter outputFile = File.AppendText("logs/leave_user.txt")) {
            await outputFile.WriteAsync($"{user.Username};{user.GlobalName}\n");
        }
    }

    public static async Task UserBanned(SocketUser user, SocketGuild guild) {
        // Load .env and get the bot channel id
        DotNetEnv.Env.Load(@"../.env");
        ulong channelId = Convert.ToUInt64(DotNetEnv.Env.GetString("TEST_CHANNEL_ID"));

        // Write flat content to a file containing user, time and reason
        using (StreamWriter outputFile = File.AppendText("logs/banned_user.txt"))
        {
            await outputFile.WriteAsync($"{user.Username}\n");
        }

        // Get the channel and check if its not null using the guard clause
        var channel = guild.GetChannel(channelId) as IMessageChannel;
        if (channel == null) {
            Console.WriteLine("Error sending message on the channel");
            return;
        }

        await channel.SendMessageAsync($"{user.Username} ({user.GlobalName}) has been banned");
    }

    public static async Task UserUnbanned(SocketUser user, SocketGuild guild) {
        // Load .env and get the bot channel id
        DotNetEnv.Env.Load(@"../.env");
        ulong channelId = Convert.ToUInt64(DotNetEnv.Env.GetString("TEST_CHANNEL_ID"));

        var channel = guild.GetChannel(channelId) as IMessageChannel;
        if (channel == null) {
            Console.WriteLine("Error sending message on the channel");
            return;
        }

        await channel.SendMessageAsync($"{user.Username} ({user.GlobalName}) has been unbanned. The ban record will not be removed");
    }
}
