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
    /// Method called when an event is created inside of the guild. It will use the announcements channel and the doof role in order to send a message with information of the event to the announcements channel tagging users with the doof rol.
    ///! Changes where made on 03/05/2024, check the CHANGELOG file for more information
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

        var embed = new EmbedBuilder {
            Title = $"A new event just dropped: {guildEvent.Name}",
            Color = Color.Green,
            Timestamp = DateTime.Now,
            Fields = {
                new EmbedFieldBuilder {
                    Name = "Time",
                    Value = guildEvent.StartTime
                }
            }
        };
        embed.WithAuthor("Moderation Team");

        if (!isEventChannelEmpty(guildEvent) && !isDescriptionEmpty(guildEvent)) {
            embed.WithDescription(guildEvent.Description);
            embed.AddField(
                name: "Channel",
                value: guildEvent.Channel
            );
        } else if (!isEventChannelEmpty(guildEvent) && isDescriptionEmpty(guildEvent)) {
            embed.AddField(
                name: "Channel",
                value: guildEvent.Channel
            );
        }

        if (!isEventLocationEmpty(guildEvent) && !isDescriptionEmpty(guildEvent)) {
            embed.WithDescription(guildEvent.Description);
            embed.AddField(
                name: "Location",
                value: guildEvent.Location
            );
        } else if (!isEventLocationEmpty(guildEvent) && isDescriptionEmpty(guildEvent)) {
            embed.AddField(
                name: "Location",
                value: guildEvent.Location
            );
        }

        await announcementsChannel.SendMessageAsync(doofRole.Mention);
        await announcementsChannel.SendMessageAsync(embed: embed.Build());
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

        if (!isEventChannelEmpty(guildEvent)) {
            await announcementsChannel.SendMessageAsync($"{doofRole.Mention}, **{guildEvent.Name}** starting at <#{guildEvent.Channel.Id}>. Hope to see you there");
        }
        else if (!isEventLocationEmpty(guildEvent)) {
            await announcementsChannel.SendMessageAsync($"{doofRole.Mention}, **{guildEvent.Name}** starting at **{guildEvent.Location}**. Hope to see you there");
        }
    }

    // Method to check if the event is created in a VC
    private bool isEventChannelEmpty(SocketGuildEvent guildEvent) {
        return guildEvent.Channel == null;
    }

    // Method to check if the event is created on a custom location
    private bool isEventLocationEmpty(SocketGuildEvent guildEvent) {
        return string.IsNullOrEmpty(guildEvent.Location);
    }
    
    // Method to check if the event contains a description
    private bool isDescriptionEmpty(SocketGuildEvent guildEvent) {
        return string.IsNullOrEmpty(guildEvent.Description);
    }
}
