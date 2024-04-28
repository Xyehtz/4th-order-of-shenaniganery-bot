using Discord;
using Discord.WebSocket;

public class ReadyBot {
    private readonly DiscordSocketClient _client;
    private readonly ulong _testChannelId = new LoadSecrets().getTestChannelId();
    private readonly ulong _generalChannelId = new LoadSecrets().getGeneralChannelId();
    private Status _status;

    public ReadyBot(DiscordSocketClient client) {
        _client = client;
        _status = new Status(client);
    }

    public async Task Ready() {
        await _status.SetGameAsync();

        var botChannel = _client.GetChannel(_testChannelId) as IMessageChannel;
        var generalChannel = _client.GetChannel(_generalChannelId) as IMessageChannel;

        if (botChannel == null || generalChannel == null)
        {
            Console.WriteLine("Error sending message to the channel(s)");
            return;
        }

        // await botChannel.SendMessageAsync($"Successfully connected to the server (Ping: {_client.Latency} ms)");
        // await generalChannel.SendMessageAsync("Perry the Platypus!");
    }
}