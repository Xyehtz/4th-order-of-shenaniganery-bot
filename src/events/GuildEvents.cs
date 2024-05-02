using Discord;
using Discord.WebSocket;

// Interface to set the methods that must be implemented in GuildEvents
public interface IGuildEvents {
    Task GuildScheduledEventCreated(SocketGuildEvent guildEvent);
    Task GuildScheduledEventStarted(SocketGuildEvent guildEvent);
}

/// <summary>
/// Class that will implement the methods inside the IGuildEvents interface. This class is in charge of controlling the events that happen inside of the guild
/// </summary>
public class GuildEvents : IGuildEvents {
    private static ulong _announcementsChannelId = new LoadSecrets().getAnnouncementsChannelId();
    private static ulong _doofRoleId = new LoadSecrets().getDoofRoleId();

    /// <summary>
    /// Method called when an event is created inside of the guild. It will use the announcements channel and the doof role in order to send a message with information of the event to the announcements channel tagging users with the doof rol
    /// </summary>
    /// <param name="guildEvent">
    /// The event that has been created along with information of it such as the time, description (if provided), the name and location
    /// </param>
    /// <returns>
    /// Task.CompletedTask and a message on the announcements channel
    /// </returns>
    public async Task GuildScheduledEventCreated(SocketGuildEvent guildEvent) {
        var announcementsChannel = guildEvent.Guild.GetChannel(_announcementsChannelId) as IMessageChannel;
        var doofRole = guildEvent.Guild.GetRole(_doofRoleId);

        if (announcementsChannel == null || doofRole == null) {
            Console.WriteLine("Error sending message on the channel or tagging the doofRole id");
            return;
        }

        //? Discord gives the mods the possibility to put or not a description, therefore is important to check if the description is empty or not, the same happens with the locations as they can change depending on if they are a voice channel or a custom location

        // TODO - Improve message formatting and make sure to check the following cases:
        /*  When a event is going to happen in a VC
            When an event happens in a custom location
            When an event contains a description
            When events don't contain a description
        */
        //* Keep in mind that at least one condition related to location and one related to description can happen at the same time

        if (guildEvent.Description == "") {
            await announcementsChannel.SendMessageAsync($"# {doofRole.Mention} a new event is coming!\n## Details\n**Name**: {guildEvent.Name}\n**Description**: No description provided\n**Start Time**: {guildEvent.StartTime}");
            return;
        }

        //? Currently, I'm thinking of a way to implement better message formats using probably a switch case and an embed

        await announcementsChannel.SendMessageAsync($"# {doofRole.Mention} a new event is coming!\n## Details\n **Name**: {guildEvent.Name}\n**Description**: {guildEvent.Description}\n**Start Time**: {guildEvent.StartTime}");
    }

    /// <summary>
    /// This method is called when an already created event is started in the guild
    /// The method retrieves the announcements channel and doof doofRole id in order to get the channel and doofRole respectively from Discord.NET
    /// It will check that the channel and the doofRole is not null and then send a message to the announcement channel tagging the members of the guild
    /// </summary>
    /// 
    /// <param name="guildEvent">
    /// Parameter sent automatically by Discord.NET when the event is triggered, it gives access to the event information and some guild information
    /// </param>
    /// 
    /// <returns>
    /// A message on the announcement channel and Task.CompletedTask
    /// </returns>
    public async Task GuildScheduledEventStarted(SocketGuildEvent guildEvent) {
        var announcementsChannel = guildEvent.Guild.GetChannel(_announcementsChannelId) as IMessageChannel;
        var doofRole = guildEvent.Guild.GetRole(_doofRoleId);

        if (announcementsChannel == null || doofRole == null) {
            Console.WriteLine("Error sending message on the channel or tagging the doofRole id");
            return;
        }

        //? Something similar as in the GuildScheduledEventCreated method happens here, this will send a bad message when the location is just a VC

        // TODO - Do the same as above
        await announcementsChannel.SendMessageAsync($"{doofRole.Mention}, **{guildEvent.Name}** has started at ***{guildEvent.Location}*** hope to see you there!");
    }
}
