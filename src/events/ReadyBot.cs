using Discord;
using Discord.WebSocket;

/// <summary>
/// This class declares what the bot will do after connecting to Discord
/// </summary>
public class ReadyBot {
    private readonly DiscordSocketClient _client;
    private readonly ulong _testChannelId = new LoadSecrets().getTestChannelId();
    private readonly ulong _generalChannelId = new LoadSecrets().getGeneralChannelId();
    private IStatus _status;

    public ReadyBot(DiscordSocketClient client) {
        _client = client;
        _status = new Status(client);
    }

    /// <summary>
    /// This method will obtain the channels of the Ids and send a different message to both of them
    /// </summary>
    /// <returns>
    /// Messages to two channels in the guild
    /// </returns>
    public async Task Ready() {
        await _status.SetGameAsync();

        var botChannel = _client.GetChannel(_testChannelId) as IMessageChannel;
        var generalChannel = _client.GetChannel(_generalChannelId) as IMessageChannel;

        if (botChannel == null || generalChannel == null)
        {
            Console.WriteLine("Error sending message to the channel(s)");
            return;
        }

        await botChannel.SendMessageAsync($"Successfully connected to the server (Ping: {_client.Latency} ms)");
        // await generalChannel.SendMessageAsync("Perry the Platypus!");
    }
}