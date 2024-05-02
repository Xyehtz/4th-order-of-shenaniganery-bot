using Discord.WebSocket;
using Discord;

// Interface with all the public methods that need to be implemented in the UserEvents class
public interface IUserEvents {
    Task UserJoined(SocketGuildUser user);
    Task UserLeft(SocketGuild guild, SocketUser user);
    Task UserBanned(SocketUser user, SocketGuild guild);
    Task UserUnbanned(SocketUser user, SocketGuild guld);
}

/// <summary>
/// This class will handle all the events related to the users with different methods depending on the type of the command
/// </summary>
public class UserEvents : IUserEvents {
    private readonly ulong _welcomeChannelId = new LoadSecrets().getWelcomeChannelId();
    private readonly ulong _modChannelId = new LoadSecrets().getModChannelId();

    /// <summary>
    /// This is the method executed when a new user joins to the guild, it will write the name of the user and the time that he joined and greet the user on the welcome channel
    /// </summary>
    /// <param name="user">
    /// The users of the event, it is given automatically by the Discord API
    /// </param>
    /// <returns>
    /// A message greeting the user and a log on the join_user.csv file
    /// </returns>
    public async Task UserJoined(SocketGuildUser user) {

        // Write content to the join_user.csv file
        using (StreamWriter outputFile = File.AppendText("logs/join_user.csv"))
        {
            await outputFile.WriteAsync($"{user.Username};{user.DisplayName};{user.JoinedAt}\n");
        }

        // Get the channel and check if its not null using the guard clause
        var welcomeChannel = user.Guild.GetChannel(_welcomeChannelId) as IMessageChannel;
        if (welcomeChannel == null) {
            Console.WriteLine("Error sending message on the channel");
            return;
        }

        // Read the the banned_user.csv file to check if the user has been banned with the if statement, if so we will count the number of times that the user has been banned
        string bannedUsers = File.ReadAllText("logs/banned_user.csv");
        if (bannedUsers.Contains(user.Username)) {

            // Set the times banned to zero, split the content of the file line by line and iterate through the usernames, each time the username appears update the counter
            int timesBanned = 0;
            string[] usernames = bannedUsers.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            foreach (string username in usernames) {
                timesBanned++;
            }

            // Check if the mod channel exist, if so, send a message to the mod channel
            var modChannel = user.Guild.GetChannel(_modChannelId) as IMessageChannel;
            if (modChannel == null) {
                Console.WriteLine("Error sending message on the channel");
                return;
            }

            await modChannel.SendMessageAsync($"{user.Username} ({user.DisplayName}) has joined the server. The user was previously banned. The user has been banned {timesBanned} time(s)");
        }

        // Send a message to the welcome channel
        await welcomeChannel.SendMessageAsync($"Hey {user.Mention} welcome to ***4th Order of Shenaniganery***");
    }

    /// <summary>
    /// This method will write to the leave_user.csv file when a user leaves the server
    /// </summary>
    /// <param name="guild">
    /// The guild of the event
    /// </param>
    /// <param name="user">
    /// The user that left the server
    /// </param>
    /// <returns>
    /// A write operation on the leave_user.csv file
    /// </returns>
    public async Task UserLeft(SocketGuild guild, SocketUser user) {
        using (StreamWriter outputFile = File.AppendText("logs/leave_user.csv")) {
            await outputFile.WriteAsync($"{user.Username};{user.GlobalName}\n");
        }
    }

    /// <summary>
    /// This method will write to the banned_user.csv file when a user is banned and also send a message to the mod channel with a confirmation of the ban
    /// </summary>
    /// <param name="user">
    /// The banned user an its information
    /// </param>
    /// <param name="guild">
    /// The guild where the user was banned
    /// </param>
    /// <returns>
    /// A write operation inside of the banned_user.csv file and a message operation on the mod channel
    /// </returns>
    public async Task UserBanned(SocketUser user, SocketGuild guild) {

        // Write flat content to a file containing user, time and reason
        using (StreamWriter outputFile = File.AppendText("logs/banned_user.csv"))
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

    /// <summary>
    /// Method called when a user ins unbanned on the guild, it will send a message on the mod channel if it is not null
    /// </summary>
    /// <param name="user">
    /// The information of the user that was unbanned
    /// </param>
    /// <param name="guild">
    /// The guild where the user was unbanned
    /// </param>
    /// <returns>
    /// A message operation on the mod channel
    /// </returns>
    public async Task UserUnbanned(SocketUser user, SocketGuild guild) {
        var modChannel = guild.GetChannel(_modChannelId) as IMessageChannel;
        if (modChannel == null) {
            Console.WriteLine("Error sending message on the channel");
            return;
        }

        await modChannel.SendMessageAsync($"{user.Username} ({user.GlobalName}) has been unbanned. The ban record will not be removed");
    }
}
