using Discord;
using Discord.WebSocket;

public class GuildEvents {
    private static ulong _announcementsChannelId;
    private static ulong _doofRoleId;

    public GuildEvents(ulong announcementsChannelId, ulong doofRoleId) {
        _announcementsChannelId = announcementsChannelId;
        _doofRoleId = doofRoleId;
    }

    /// <summary>
    /// This method is called when an event is created inside of the guild.
    /// The method will get the announcements channel and the doof doofRole in order to send an announcement tagging the members of the guild. 
    /// The message will display the name, description (if provided) and the start time of the event
    /// The method has a guard clause to ensure that the channel and the doofRole information is successfully retrieved
    /// </summary>
    /// 
    /// <param name="guildEvent">
    /// This parameter is sent automatically by Discord.NET when the event is triggered
    /// This parameter gives access to the event information and also some guild information
    /// </param>
    /// 
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

        if (guildEvent.Description == "") {
            await announcementsChannel.SendMessageAsync($"# {doofRole.Mention} a new event is coming!\n## Details\n**Name**: {guildEvent.Name}\n**Description**: No description provided\n**Start Time**: {guildEvent.StartTime}");
            return;
        }

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

        await announcementsChannel.SendMessageAsync($"{doofRole.Mention}, **{guildEvent.Name}** has started at ***{guildEvent.Location}*** hope to see you there!");
    }
}
